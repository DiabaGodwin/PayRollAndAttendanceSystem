using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto.AttendanceRecord;


public class UpdatedAttendanceRequest
{
    [Required]
    public int EmployeeId { get; set; }

}