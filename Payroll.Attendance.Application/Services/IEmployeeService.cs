using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IEmployeeService
{
    Task<ApiResponse<int>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token);
    Task<ApiResponse<EmployeeResponseDto>> GetAllEmployeesAsync(EmployeeResponseDto cancellationToken);

    Task<Employee> UpdateEmployeeAsync(Employee updateEmployeeDto,
        CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
}