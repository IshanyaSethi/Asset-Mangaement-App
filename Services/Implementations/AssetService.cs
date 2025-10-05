using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Data;
using AssetManagementApp.Data.Models;
using AssetManagementApp.Services.Interfaces;

namespace AssetManagementApp.Services.Implementations
{
    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _context;

        public AssetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Asset>> GetAllAssetsAsync()
        {
            return await _context.Assets
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await _context.Assets.FindAsync(id);
        }

        public async Task<List<Asset>> GetAvailableAssetsAsync()
        {
            return await _context.Assets
                .Where(a => a.Status == "Available")
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }

        public async Task<List<Asset>> GetAssetsByStatusAsync(string status)
        {
            return await _context.Assets
                .Where(a => a.Status == status)
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }

        public async Task<List<Asset>> GetAssetsByTypeAsync(string type)
        {
            return await _context.Assets
                .Where(a => a.AssetType == type)
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }

        public async Task<Asset> AddAssetAsync(Asset asset)
        {
            // Check if serial number already exists
            var existingAsset = await _context.Assets
                .FirstOrDefaultAsync(a => a.SerialNumber == asset.SerialNumber);

            if (existingAsset != null)
            {
                throw new InvalidOperationException("An asset with this serial number already exists.");
            }

            // Validate warranty date
            if (asset.WarrantyExpiryDate.HasValue && asset.WarrantyExpiryDate < asset.PurchaseDate)
            {
                throw new InvalidOperationException("Warranty expiry date cannot be before purchase date.");
            }

            // Set initial status to Available for new assets
            asset.Status = "Available";

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> UpdateAssetAsync(Asset asset)
        {
            var existingAsset = await _context.Assets.FindAsync(asset.AssetId);

            if (existingAsset == null)
            {
                throw new InvalidOperationException("Asset not found.");
            }

            // Check if serial number is being changed to one that already exists
            var duplicateSerial = await _context.Assets
                .AnyAsync(a => a.SerialNumber == asset.SerialNumber && a.AssetId != asset.AssetId);

            if (duplicateSerial)
            {
                throw new InvalidOperationException("An asset with this serial number already exists.");
            }

            // Validate warranty date
            if (asset.WarrantyExpiryDate.HasValue && asset.WarrantyExpiryDate < asset.PurchaseDate)
            {
                throw new InvalidOperationException("Warranty expiry date cannot be before purchase date.");
            }

            existingAsset.AssetName = asset.AssetName;
            existingAsset.AssetType = asset.AssetType;
            existingAsset.MakeModel = asset.MakeModel;
            existingAsset.SerialNumber = asset.SerialNumber;
            existingAsset.PurchaseDate = asset.PurchaseDate;
            existingAsset.WarrantyExpiryDate = asset.WarrantyExpiryDate;
            existingAsset.Condition = asset.Condition;
            existingAsset.IsSpare = asset.IsSpare;
            existingAsset.Specifications = asset.Specifications;
            // Note: Status is NOT updated here, use ChangeAssetStatusAsync for that

            await _context.SaveChangesAsync();
            return existingAsset;
        }

        public async Task DeleteAssetAsync(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssetAssignments)
                .FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null)
            {
                throw new InvalidOperationException("Asset not found.");
            }

            // Check if asset is currently assigned
            var hasActiveAssignment = asset.AssetAssignments.Any(aa => aa.ReturnedDate == null);

            if (hasActiveAssignment)
            {
                throw new CannotDeleteException(
                    $"Cannot delete asset '{asset.AssetName}'. " +
                    "This asset is currently assigned to an employee. " +
                    "Please return the asset before deleting.");
            }

            // Check if asset has any historical assignments
            if (asset.AssetAssignments.Any())
            {
                throw new CannotDeleteException(
                    $"Cannot delete asset '{asset.AssetName}'. " +
                    $"This asset has {asset.AssetAssignments.Count} assignment record(s). " +
                    "Please mark as Retired instead.");
            }

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeAssetStatusAsync(int id, string newStatus)
        {
            var asset = await _context.Assets.FindAsync(id);

            if (asset == null)
            {
                throw new InvalidOperationException("Asset not found.");
            }

            // Validate status transitions
            var validStatuses = new[] { "Available", "Assigned", "Under Repair", "Retired" };
            if (!validStatuses.Contains(newStatus))
            {
                throw new InvalidOperationException($"Invalid status: {newStatus}");
            }

            // Business rule: Can't manually set to "Assigned" (must use assignment process)
            if (newStatus == "Assigned")
            {
                throw new InvalidOperationException("Cannot manually set status to 'Assigned'. Use the assignment process instead.");
            }

            asset.Status = newStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Asset>> SearchAssetsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAssetsAsync();
            }

            searchTerm = searchTerm.ToLower();

            return await _context.Assets
                .Where(a => a.AssetName.ToLower().Contains(searchTerm) ||
                           a.AssetType.ToLower().Contains(searchTerm) ||
                           a.SerialNumber.ToLower().Contains(searchTerm) ||
                           (a.MakeModel != null && a.MakeModel.ToLower().Contains(searchTerm)))
                .OrderBy(a => a.AssetName)
                .ToListAsync();
        }

        public async Task<List<string>> GetAssetTypesAsync()
        {
            return await _context.Assets
                .Select(a => a.AssetType)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }
    }
}