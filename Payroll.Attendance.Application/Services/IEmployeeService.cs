using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;

namespace Payroll.Attendance.Application.Services;

public interface IEmployeeService
{
    Task<ApiResponse<EmployeeResponseDto>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token);

    Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync(PaginationRequest request,
        CancellationToken cancellationToken);

    Task<ApiResponse<bool>> UpdateEmployeeAsync(int id,UpdateEmployeeRequest
        request,CancellationToken cancellationToken);

    Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id,
        CancellationToken cancellationToken);

    Task<ApiResponse<List<EmployeeIdAndNameDto>>> GetEmployeeIdAndName(string? searchText, CancellationToken cancellationToken);
    Task<ApiResponse<List<EmployeeResponseDto>>> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<EmployeeSummaryDto>> GetEmployeeSummaryAsync(CancellationToken cancellationToken);
    Task<ApiResponse<EmployeeAnalyticsDto>> GetEmployeeAnalyticsAsync(int employeeId, string searchText,
        DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken);
}