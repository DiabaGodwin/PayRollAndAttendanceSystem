namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class BulkAttendanceRequestDto
{
    public DateTime Date { get; set; }
    public List<BulkAttendanceRecordDto> Records { get; set; } = new();
}