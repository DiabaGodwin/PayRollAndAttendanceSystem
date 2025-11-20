namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class AttendanceResponseDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public string FirstName { get; set; }= string.Empty;
    public string Surname { get; set; }=string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? Department { get; set; }

    public TimeSpan? TotalHours { get; set; }
    public bool IsLate { get; set; }
    public bool LeftEarly { get; set; }
   
}