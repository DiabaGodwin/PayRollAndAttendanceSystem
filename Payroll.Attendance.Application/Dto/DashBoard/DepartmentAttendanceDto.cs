namespace Payroll.Attendance.Application.Dto.DashBoard;

public class DepartmentAttendanceDto
{
    public string? DepartmentName { get; set; }
    public int EmployeeCount { get; set; }
    public decimal PresentCount { get; set; }

}