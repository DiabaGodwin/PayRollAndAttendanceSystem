using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto.DashBoard;

public class FullDashboardResponseDto
{
    public DashBoardSummaryDto? Summary { get; set; } = new();
    public List<PayrollTrendDto> PayrollTrend { get; set; } = new();
    public List<ReminderDto> Reminders { get; set; } = new();
    
    public int TotalEmployees { get; set; }
    public IEnumerable<DepartmentDistributionDto> DepartmentDistribution { get; set; }
    public int PendingApprovals { get; set; }
    public int PendingReminders { get; set; }
    
    public DateTime LastUpdated { get; set; }
}