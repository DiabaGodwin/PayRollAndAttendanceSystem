using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IEmployeeService
{
    Task<ApiResponse<int>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token);

    Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync(PaginationRequest request,
        CancellationToken cancellationToken);

    Task<Employee> UpdateEmployeeAsync(Employee updateEmployeeDto,
        CancellationToken cancellationToken);

    Task<ApiResponse<List<EmployeeResponseDto>>> GetEmployeeByIdAsync(int id,
        CancellationToken cancellationToken);

    Task<ApiResponse<List<EmployeeIdAndNameDto>>> GetEmployeeIdAndName(string? searchText, CancellationToken cancellationToken);
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<EmployeeSummaryDto>> GetEmployeeSummaryAsync(CancellationToken cancellationToken);
}