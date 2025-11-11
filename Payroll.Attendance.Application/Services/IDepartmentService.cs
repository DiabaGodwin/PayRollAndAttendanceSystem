using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Department;
using Payroll.Attendance.Application.Dto.Employee;

namespace Payroll.Attendance.Application.Services;

public interface IDepartmentService
{
    Task<ApiResponse<int>> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto, CancellationToken cancellationToken);
    Task<ApiResponse<List<DepartmentResponseDto>>> GetAllDepartmentsAsync(bool includeEmployees = false,
        CancellationToken cancellationToken = default);
    Task<ApiResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(int id, bool includeEmployees = false, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> UpdateDepartmentAsync(UpdateDepartmentRequest updateDepartmentRequest,
        CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteDepartmentAsync(int id, CancellationToken cancellationToken);

    Task<ApiResponse<List<GetOnlyDepartmentDto>>> GetAllOnlyDepartmentsAsync(
        CancellationToken cancellationToken = default);

}