using AssetManagementApp.Data.Models;

namespace AssetManagementApp.Services.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> ExportAssetsToCSVAsync();
        Task<byte[]> ExportEmployeesToCSVAsync();
        Task<byte[]> ExportAssignmentsToCSVAsync();
        Task<List<AssetAssignment>> GetAssignmentHistoryAsync(DateTime? startDate, DateTime? endDate);
        Task<List<AssetAssignment>> GetEmployeeAssignmentHistoryAsync(int employeeId);
        Task<List<AssetAssignment>> GetAssetAssignmentHistoryAsync(int assetId);
    }
}