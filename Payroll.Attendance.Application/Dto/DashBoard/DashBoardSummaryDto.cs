using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto.DashBoard;

public class DashBoardSummaryDto
{
    public int TotalEmployees { get; set; }
    public decimal AttendanceRate { get; set; }
    public decimal TotalPayroll { get; set; }

}