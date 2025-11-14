namespace Payroll.Attendance.Application.Dto.AttendanceRecord
{
    public class AttendanceSummaryDto
    {
        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int LateArrivals { get; set; }
        public int Absent { get; set; }
        
     
     
    }
}