namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class BulkAttendanceRecordDto
{
    public int EmployeeId { get; set; }
    public bool CheckIn { get; set; }
    public bool CheckOut { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}