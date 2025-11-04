using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IEmployeeRepository
    {
      
        Task<int> AddEmployeeAsync(Employee employee, CancellationToken  token);
        
        Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken);
        
        Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<Employee> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    
        Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<Employee> GetByIdAsync(int employeeId);
        Task<Employee> GetByEmailAsync(string Email, CancellationToken ct);
        Task<EmployeeSummary?> GetByEmployeeSummaryAsync(string username, CancellationToken ct);
        Task<List<Employee>> GetEmployeesByDepartmentAsync(string department, CancellationToken ct);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(CancellationToken ct);
    }
}