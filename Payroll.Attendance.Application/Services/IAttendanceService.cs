using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;
public interface IAttendanceService
{
    Task<ApiResponse<int>> CheckIn(AttendanceRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<List<AttendanceResponseDto>>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<AttendanceRecord?>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<int>> CheckOutAsync(UpdatedAttendanceRequest request, CancellationToken cancellationToken);

    Task<ApiResponse<int>> DeleteAsync(int id, CancellationToken cancellationToken);
    
    Task<ApiResponse<AttendanceSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken);
}
