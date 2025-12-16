namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class TodayAttendanceSummaryDto
{
    public int TotalEmployee { get; set; }
    public DateTime Date { get; set; }
    public string? DayName { get; set; }
    public int PresentToday { get; set; }
    public int AbsentToday { get; set; }
    public int LateArrivals { get; set; }
    public int OnLeave { get; set; }
    public double AttendancePercentage { get; set; }
}