using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Department;
using Payroll.Attendance.Application.Dto.Employee;

namespace Payroll.Attendance.Application.Services;

public interface IDepartmentService
{
    Task<ApiResponse<int>> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto, CancellationToken cancellationToken);
    Task<ApiResponse<List<DepartmentResponseDto>>> GetAllDepartmentsAsync(PaginationRequest request,
        CancellationToken cancellationToken );
    Task<ApiResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken );
    Task<ApiResponse<bool>> UpdateDepartmentAsync(UpdateDepartmentRequest updateDepartmentRequest,
        CancellationToken cancellationToken);
    Task<ApiResponse<bool>> DeleteDepartmentAsync(int id, CancellationToken cancellationToken);

    Task<ApiResponse<List<GetOnlyDepartmentDto>>> GetAllOnlyDepartmentsAsync(
        CancellationToken cancellationToken );

}