namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class WeekAttendanceSummaryDto
{
    public int TotalEmployee { get; set; }
    
    public int PresentWeek { get; set; }
    public int AbsentWeek { get; set; }
    public int WeekLateArrivals { get; set; }
    public int WeekOnLeave { get; set; }
    public double WeeklyAttendancePercentage { get; set; }
    public List<TodayAttendanceSummaryDto> DialyBreak { get; set; }
    
}

