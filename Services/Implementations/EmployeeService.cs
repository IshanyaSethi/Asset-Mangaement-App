using Microsoft.EntityFrameworkCore;
using AssetManagementApp.Data;
using AssetManagementApp.Data.Models;
using AssetManagementApp.Services.Interfaces;

namespace AssetManagementApp.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            // Check if email already exists
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employee.Email);

            if (existingEmployee != null)
            {
                throw new InvalidOperationException("An employee with this email already exists.");
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);
            
            if (existingEmployee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            // Check if email is being changed to one that already exists
            var duplicateEmail = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email && e.EmployeeId != employee.EmployeeId);

            if (duplicateEmail)
            {
                throw new InvalidOperationException("An employee with this email already exists.");
            }

            existingEmployee.FullName = employee.FullName;
            existingEmployee.Department = employee.Department;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.IsActive = employee.IsActive;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.AssetAssignments)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            // Check if employee has any assignments (active or historical)
            if (employee.AssetAssignments.Any())
            {
                throw new CannotDeleteException(
                    $"Cannot delete employee '{employee.FullName}'. " +
                    $"This employee has {employee.AssetAssignments.Count} assignment record(s). " +
                    "Please mark as inactive instead.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteEmployeeAsync(int id)
        {
            var hasAssignments = await _context.AssetAssignments
                .AnyAsync(aa => aa.EmployeeId == id);

            return !hasAssignments;
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllEmployeesAsync();
            }

            searchTerm = searchTerm.ToLower();

            return await _context.Employees
                .Where(e => e.FullName.ToLower().Contains(searchTerm) ||
                           e.Department.ToLower().Contains(searchTerm) ||
                           e.Email.ToLower().Contains(searchTerm) ||
                           e.Designation.ToLower().Contains(searchTerm))
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }
    }
}