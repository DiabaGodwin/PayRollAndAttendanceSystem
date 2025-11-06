using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IEmployeeRepository
    {
      
        Task<int> AddEmployeeAsync(Employee employee, CancellationToken  token);
        
        Task<List<Employee>> GetAllEmployeesAsync(PaginationRequest request, CancellationToken cancellationToken);
        
        Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<Employee> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    
        Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
        /*
        
        Task<Employee> GetByIdAsync(int employeeId);
        Task<Employee> GetByEmailAsync(string Email, CancellationToken ct);
        Task<EmployeeSummary?> GetByEmployeeSummaryAsync( string sername, CancellationToken ct);
        Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken ct);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(CancellationToken ct);
        */
        Task<Employee?> GetEmployeeBasicByIdAsync(int id, CancellationToken cancellationToken);
       
    }
}