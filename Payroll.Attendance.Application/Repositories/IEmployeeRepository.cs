using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IEmployeeRepository
    {
      
        Task<int> AddEmployeeAsync(Employee employee, CancellationToken  token);
        
        Task<List<Employee>> GetAllEmployeesAsync(PaginationRequest request, CancellationToken cancellationToken);
        
        Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<bool> UpdateEmployeeAsync(int employee, UpdateEmployeeRequest request, CancellationToken cancellationToken);
    
        Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
      
        Task<List<EmployeeIdAndNameDto>> GetEmployeeIdAndName(string? searchText, CancellationToken cancellationToken);
       


        Task<EmployeeSummary?> GetEmployeeSummaryAsync(CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    }
}