using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Data;
using AssetManagementApp.Data.Models;
using AssetManagementApp.Services.Interfaces;

namespace AssetManagementApp.Services.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetAssignment>> GetAllAssignmentsAsync()
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetActiveAssignmentsAsync()
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.ReturnedDate == null)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetAssignmentsByEmployeeAsync(int employeeId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.EmployeeId == employeeId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetAssignmentsByAssetAsync(int assetId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.AssetId == assetId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<AssetAssignment> AssignAssetAsync(int assetId, int employeeId, string? notes)
        {
            // Start a transaction to ensure both changes succeed or fail together
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get asset
                var asset = await _context.Assets.FindAsync(assetId);
                if (asset == null)
                {
                    throw new InvalidOperationException("Asset not found.");
                }

                // Check if asset is available
                if (asset.Status != "Available")
                {
                    throw new AssetNotAvailableException(
                        $"Asset '{asset.AssetName}' is not available. Current status: {asset.Status}");
                }

                // Get employee
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    throw new InvalidOperationException("Employee not found.");
                }

                // Check if employee is active
                if (!employee.IsActive)
                {
                    throw new EmployeeNotActiveException(
                        $"Employee '{employee.FullName}' is not active. Cannot assign assets to inactive employees.");
                }

                // Check if asset already has an active assignment (safety check)
                var existingActiveAssignment = await _context.AssetAssignments
                    .AnyAsync(aa => aa.AssetId == assetId && aa.ReturnedDate == null);

                if (existingActiveAssignment)
                {
                    throw new AssetAlreadyAssignedException(
                        $"Asset '{asset.AssetName}' is already assigned to another employee.");
                }

                // Create assignment record
                var assignment = new AssetAssignment
                {
                    AssetId = assetId,
                    EmployeeId = employeeId,
                    AssignedDate = DateTime.Now,
                    ReturnedDate = null,
                    Notes = notes
                };

                _context.AssetAssignments.Add(assignment);

                // Update asset status
                asset.Status = "Assigned";

                // Save changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                // Reload with navigation properties
                await _context.Entry(assignment).Reference(a => a.Asset).LoadAsync();
                await _context.Entry(assignment).Reference(a => a.Employee).LoadAsync();

                return assignment;
            }
            catch
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ReturnAssetAsync(int assignmentId)
        {
            // Start a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get assignment
                var assignment = await _context.AssetAssignments
                    .Include(aa => aa.Asset)
                    .FirstOrDefaultAsync(aa => aa.AssignmentId == assignmentId);

                if (assignment == null)
                {
                    throw new InvalidOperationException("Assignment not found.");
                }

                // Check if already returned
                if (assignment.ReturnedDate.HasValue)
                {
                    throw new InvalidOperationException(
                        $"This asset was already returned on {assignment.ReturnedDate:yyyy-MM-dd}.");
                }

                // Set return date
                assignment.ReturnedDate = DateTime.Now;

                // Update asset status back to Available
                assignment.Asset.Status = "Available";

                // Save changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AssetAssignment?> GetAssignmentByIdAsync(int id)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .FirstOrDefaultAsync(aa => aa.AssignmentId == id);
        }
    }
}