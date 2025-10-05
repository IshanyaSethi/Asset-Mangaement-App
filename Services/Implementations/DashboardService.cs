using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using AssetManagementApp.Services.Interfaces;

namespace AssetManagementApp.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var stats = new DashboardStats();

            // Asset statistics
            stats.TotalAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets");

            stats.AvailableAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets WHERE Status = 'Available'");

            stats.AssignedAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets WHERE Status = 'Assigned'");

            stats.UnderRepairAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets WHERE Status = 'Under Repair'");

            stats.RetiredAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets WHERE Status = 'Retired'");

            stats.SpareAssets = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Assets WHERE IsSpare = 1");

            // Employee statistics
            stats.TotalEmployees = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Employees");

            stats.ActiveEmployees = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Employees WHERE IsActive = 1");

            // Assignment statistics
            stats.ActiveAssignments = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM AssetAssignments WHERE ReturnedDate IS NULL");

            stats.TotalAssignments = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM AssetAssignments");

            return stats;
        }

        public async Task<List<WarrantyAlert>> GetWarrantyAlertsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    AssetId,
                    AssetName,
                    SerialNumber,
                    WarrantyExpiryDate,
                    CAST((julianday(WarrantyExpiryDate) - julianday('now')) AS INTEGER) as DaysUntilExpiry
                FROM Assets
                WHERE WarrantyExpiryDate IS NOT NULL
                  AND WarrantyExpiryDate >= date('now')
                  AND WarrantyExpiryDate <= date('now', '+90 days')
                  AND Status != 'Retired'
                ORDER BY WarrantyExpiryDate ASC
                LIMIT 10";

            var alerts = await connection.QueryAsync<WarrantyAlert>(sql);
            return alerts.ToList();
        }

        public async Task<List<AssetTypeCount>> GetAssetsByTypeAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    AssetType,
                    COUNT(*) as Count
                FROM Assets
                GROUP BY AssetType
                ORDER BY Count DESC";

            var results = await connection.QueryAsync<AssetTypeCount>(sql);
            return results.ToList();
        }
    }
}