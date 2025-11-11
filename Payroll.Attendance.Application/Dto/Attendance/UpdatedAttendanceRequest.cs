using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;


public class UpdatedAttendanceRequest
{
    [Required]
    public int EmployeeId { get; set; }

}