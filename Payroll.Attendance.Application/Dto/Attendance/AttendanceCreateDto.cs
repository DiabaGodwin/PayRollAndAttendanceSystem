namespace Payroll.Attendance.Application.Dto.Attendance;

public class AttendanceCreateDto
{
    public int EmployeeId { get; set; }

    public DateTime? CheckIn { get; set; }    
    public DateTime? CheckOut { get; set; }
    
}