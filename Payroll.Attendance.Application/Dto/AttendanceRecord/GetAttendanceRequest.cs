namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class GetAttendanceRequest : PaginationRequest
{
    public int? EmployeeId { get; set; }
}