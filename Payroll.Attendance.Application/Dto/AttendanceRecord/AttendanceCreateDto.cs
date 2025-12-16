namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class AttendanceCreateDto
{
    public int EmployeeId { get; set; }

    public DateTime? CheckIn { get; set; }    
    public DateTime? CheckOut { get; set; }
    
}