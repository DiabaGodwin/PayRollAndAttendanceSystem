using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.DashBoard;

namespace Payroll.Attendance.Application.Services;

public interface IDashboardService
{
    Task<ApiResponse<DashBoardSummaryDto>> GetDashboardSummaryAsync(string userName, CancellationToken cancellationToken);
    Task<List<ApiResponse<PayrollTrendDto>>> GetPayrollTrendAsync(string selectedMonth, CancellationToken cancellationToken);
    Task<List<ApiResponse<DepartmentDistributionDto>>> GetDepartmentDistributionAsync(string selectedDepartment, CancellationToken cancellationToken);
}