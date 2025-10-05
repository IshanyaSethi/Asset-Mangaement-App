using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Data;
using AssetManagementApp.Data.Models;
using AssetManagementApp.Services.Interfaces;

namespace AssetManagementApp.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportAssetsToCSVAsync()
        {
            var assets = await _context.Assets
                .OrderBy(a => a.AssetName)
                .ToListAsync();

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

            // Write headers
            csvWriter.WriteField("Asset ID");
            csvWriter.WriteField("Asset Name");
            csvWriter.WriteField("Type");
            csvWriter.WriteField("Make/Model");
            csvWriter.WriteField("Serial Number");
            csvWriter.WriteField("Purchase Date");
            csvWriter.WriteField("Warranty Expiry");
            csvWriter.WriteField("Condition");
            csvWriter.WriteField("Status");
            csvWriter.WriteField("Is Spare");
            csvWriter.WriteField("Specifications");
            csvWriter.NextRecord();

            // Write data
            foreach (var asset in assets)
            {
                csvWriter.WriteField(asset.AssetId);
                csvWriter.WriteField(asset.AssetName);
                csvWriter.WriteField(asset.AssetType);
                csvWriter.WriteField(asset.MakeModel ?? "");
                csvWriter.WriteField(asset.SerialNumber);
                csvWriter.WriteField(asset.PurchaseDate.ToString("yyyy-MM-dd"));
                csvWriter.WriteField(asset.WarrantyExpiryDate?.ToString("yyyy-MM-dd") ?? "");
                csvWriter.WriteField(asset.Condition);
                csvWriter.WriteField(asset.Status);
                csvWriter.WriteField(asset.IsSpare ? "Yes" : "No");
                csvWriter.WriteField(asset.Specifications ?? "");
                csvWriter.NextRecord();
            }

            await streamWriter.FlushAsync();
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportEmployeesToCSVAsync()
        {
            var employees = await _context.Employees
                .OrderBy(e => e.FullName)
                .ToListAsync();

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

            // Write headers
            csvWriter.WriteField("Employee ID");
            csvWriter.WriteField("Full Name");
            csvWriter.WriteField("Department");
            csvWriter.WriteField("Email");
            csvWriter.WriteField("Phone Number");
            csvWriter.WriteField("Designation");
            csvWriter.WriteField("Status");
            csvWriter.NextRecord();

            // Write data
            foreach (var emp in employees)
            {
                csvWriter.WriteField(emp.EmployeeId);
                csvWriter.WriteField(emp.FullName);
                csvWriter.WriteField(emp.Department);
                csvWriter.WriteField(emp.Email);
                csvWriter.WriteField(emp.PhoneNumber ?? "");
                csvWriter.WriteField(emp.Designation);
                csvWriter.WriteField(emp.IsActive ? "Active" : "Inactive");
                csvWriter.NextRecord();
            }

            await streamWriter.FlushAsync();
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportAssignmentsToCSVAsync()
        {
            var assignments = await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

            // Write headers
            csvWriter.WriteField("Assignment ID");
            csvWriter.WriteField("Asset Name");
            csvWriter.WriteField("Serial Number");
            csvWriter.WriteField("Employee Name");
            csvWriter.WriteField("Department");
            csvWriter.WriteField("Assigned Date");
            csvWriter.WriteField("Returned Date");
            csvWriter.WriteField("Duration (Days)");
            csvWriter.WriteField("Status");
            csvWriter.WriteField("Notes");
            csvWriter.NextRecord();

            // Write data
            foreach (var assignment in assignments)
            {
                var endDate = assignment.ReturnedDate ?? DateTime.Now;
                var duration = (endDate - assignment.AssignedDate).Days;
                
                csvWriter.WriteField(assignment.AssignmentId);
                csvWriter.WriteField(assignment.Asset.AssetName);
                csvWriter.WriteField(assignment.Asset.SerialNumber);
                csvWriter.WriteField(assignment.Employee.FullName);
                csvWriter.WriteField(assignment.Employee.Department);
                csvWriter.WriteField(assignment.AssignedDate.ToString("yyyy-MM-dd"));
                csvWriter.WriteField(assignment.ReturnedDate?.ToString("yyyy-MM-dd") ?? "");
                csvWriter.WriteField(duration);
                csvWriter.WriteField(assignment.ReturnedDate.HasValue ? "Returned" : "Active");
                csvWriter.WriteField(assignment.Notes ?? "");
                csvWriter.NextRecord();
            }

            await streamWriter.FlushAsync();
            return memoryStream.ToArray();
        }

        public async Task<List<AssetAssignment>> GetAssignmentHistoryAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(aa => aa.AssignedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(aa => aa.AssignedDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetEmployeeAssignmentHistoryAsync(int employeeId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.EmployeeId == employeeId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<AssetAssignment>> GetAssetAssignmentHistoryAsync(int assetId)
        {
            return await _context.AssetAssignments
                .Include(aa => aa.Asset)
                .Include(aa => aa.Employee)
                .Where(aa => aa.AssetId == assetId)
                .OrderByDescending(aa => aa.AssignedDate)
                .ToListAsync();
        }
    }
}