using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.DashBoard;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IDashboardService
{
    Task<ApiResponse<DashBoardSummaryDto>> GetDashboardSummaryAsync(
        CancellationToken cancellationToken);
    Task<ApiResponse<List<PayrollTrendDto>>> GetPayrollTrendAsync(
        CancellationToken cancellationToken);
    Task<ApiResponse<List<DepartmentDistributionDto>>> GetDepartmentDistributionAsync(
        CancellationToken cancellationToken);
    Task<ApiResponse<List<ReminderDto>>> CreatReminderAsync(CreateReminderRequest request,
        CancellationToken cancellationToken);
    Task<ApiResponse<int>> DeleteReminderAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<FullDashboardResponseDto>> GetFullDashboardAsync(
        CancellationToken cancellationToken);
    Task<ApiResponse<List<ActivityDto>>> GetActivityAsync(CancellationToken cancellationToken);
    

}