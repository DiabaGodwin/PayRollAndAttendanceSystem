namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class BulkAttendanceRecordDto
{
    public int EmployeeId { get; set; }
   
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}