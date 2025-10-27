using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IEmployeeRepository
    {
      
        Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

        
        Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken);

        
        Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);

      
        Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);

    
        Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        Task<object> GetByIdAsync(int employeeId);
        Task<object> GetByIdAsync(string employeeId, CancellationToken ct);
    }
}