using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;
public interface IAttendanceService
{
    Task<ApiResponse<int>> CheckIn(AttendanceRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<List<AttendanceResponseDto>>> GetAllAsync( GetAttendanceRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<AttendanceResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<int>> CheckOutAsync(UpdatedAttendanceRequest request, CancellationToken cancellationToken);

    Task<ApiResponse<int>> DeleteAsync(int id, CancellationToken cancellationToken);
    
    Task<ApiResponse<AttendanceSummaryDto>> GetAllSummaryAsync(
        CancellationToken cancellationToken);
    Task<decimal> GetOverallAttendanceRateAsync(int month, int year, CancellationToken cancellationToken);
    Task<List<DepartmentAttendance>> GetDepartmentAttendanceAsync(CancellationToken cancellationToken);
 
    Task<ApiResponse<BulkAttendanceResponseDto>> BulkAttendanceAsync(BulkAttendanceRequestDto request,
        CancellationToken cancellationToken);
    Task<ApiResponse<List<AttendanceResponseDto>>> GetTodayAttendanceAsync( CancellationToken cancellationToken);
    Task<ApiResponse<List<TodayAttendanceDto>>> GetTodayAttendanceWithoutTokenAsync(CancellationToken cancellationToken);
    Task<ApiResponse<TodayAttendanceSummaryDto>> GetOnlyTodayAttendanceSummaryAsync(DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken);
    Task<ApiResponse<WeekAttendanceSummaryDto>> GetOnlyWeekAttendanceSummaryAsync(DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken);
    Task<ApiResponse<MonthAttendanceSummaryDto>> GetOnlyMonthAttendanceSummaryAsync(DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken);
    Task MarkMissedCheckoutsAsync (CancellationToken cancellationToken);
    
} 
