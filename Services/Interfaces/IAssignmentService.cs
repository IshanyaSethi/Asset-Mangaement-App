using AssetManagementApp.Data.Models;

namespace AssetManagementApp.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<AssetAssignment>> GetAllAssignmentsAsync();
        Task<List<AssetAssignment>> GetActiveAssignmentsAsync();
        Task<List<AssetAssignment>> GetAssignmentsByEmployeeAsync(int employeeId);
        Task<List<AssetAssignment>> GetAssignmentsByAssetAsync(int assetId);
        Task<AssetAssignment> AssignAssetAsync(int assetId, int employeeId, string? notes);
        Task ReturnAssetAsync(int assignmentId);
        Task<AssetAssignment?> GetAssignmentByIdAsync(int id);
    }
}