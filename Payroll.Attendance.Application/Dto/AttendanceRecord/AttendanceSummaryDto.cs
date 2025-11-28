namespace Payroll.Attendance.Application.Dto.AttendanceRecord
{
    public class AttendanceSummaryDto
    {
        public TodaySummaryDto Today { get; set; } = new();
        public MonthSummaryDto Month { get; set; } = new();
        public AllTimeSummaryDto AllTime { get; set; } = new();
    }

    public class TodaySummaryDto
    {
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public int LateArrivals { get; set; }
        public int OnLeave { get; set; }
        public double AttendancePercentage { get; set; }
    }

    public class MonthSummaryDto
    {
        public  int WorkingDays { get; set; }
        public int OverallPresent { get; set; }
        public int OverallAbsent { get; set; }
        public int OverallLeave { get; set; }
        public double AverageAttendancePercentage { get; set; }
    }

    public class AllTimeSummaryDto
    {
        public int AllWorkingDays { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLeave { get; set; }
        public double LifetimeAttendancePercentage { get; set; }
    }
}