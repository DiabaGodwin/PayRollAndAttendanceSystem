namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class MonthAttendanceSummaryDto
{
    public int TotalEmployee { get; set; }
    public  int WorkingDays { get; set; }
    public int OverallPresent { get; set; }
    public int OverallAbsent { get; set; }
    public int OverallLeave { get; set; }
    public double AverageAttendancePercentage { get; set; }
}