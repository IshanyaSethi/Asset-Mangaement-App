using AssetManagementApp.Data.Models;

namespace AssetManagementApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetActiveEmployeesAsync();
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<bool> CanDeleteEmployeeAsync(int id);
        Task<List<Employee>> SearchEmployeesAsync(string searchTerm);
    }
}