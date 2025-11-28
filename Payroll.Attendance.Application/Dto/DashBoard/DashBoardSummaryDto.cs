using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto.DashBoard;

public class DashBoardSummaryDto
{
    
    public string WelcomeMessage { get; set; } = null!;
    public List<KeyMetric> KeyMetrics { get; set; } = new();
    public List<DepartmentAttendance> DepartmentAttendances { get; set; } = new();
    public List<Reminder> Reminders { get; set; } = new();
    public List<Activity> Activities { get; set; } = new();
    public List<PayrollTrend> PayrollTrends { get; set; } = new();
    public List<DepartmentDistribution> DepartmentDistributions { get; set; } = new();

}