namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class TodayAttendanceDto
{
    public int EmployeeId { get; set; }
   
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? JobPosition { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public bool IsLate {get; set;}
}