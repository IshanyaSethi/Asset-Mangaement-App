using AssetManagementApp.Data.Models;

namespace AssetManagementApp.Services.Interfaces
{
    public interface IAssetService
    {
        Task<List<Asset>> GetAllAssetsAsync();
        Task<Asset?> GetAssetByIdAsync(int id);
        Task<List<Asset>> GetAvailableAssetsAsync();
        Task<List<Asset>> GetAssetsByStatusAsync(string status);
        Task<List<Asset>> GetAssetsByTypeAsync(string type);
        Task<Asset> AddAssetAsync(Asset asset);
        Task<Asset> UpdateAssetAsync(Asset asset);
        Task DeleteAssetAsync(int id);
        Task ChangeAssetStatusAsync(int id, string newStatus);
        Task<List<Asset>> SearchAssetsAsync(string searchTerm);
        Task<List<string>> GetAssetTypesAsync();
    }
}