namespace AssetManagementApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStats> GetDashboardStatsAsync();
        Task<List<WarrantyAlert>> GetWarrantyAlertsAsync();
        Task<List<AssetTypeCount>> GetAssetsByTypeAsync();
    }

    public class DashboardStats
    {
        public int TotalAssets { get; set; }
        public int AvailableAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int UnderRepairAssets { get; set; }
        public int RetiredAssets { get; set; }
        public int SpareAssets { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int ActiveAssignments { get; set; }
        public int TotalAssignments { get; set; }
    }

    public class WarrantyAlert
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime? WarrantyExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
    }

    public class AssetTypeCount
    {
        public string AssetType { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}