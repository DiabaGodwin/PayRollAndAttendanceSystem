namespace Payroll.Attendance.Application.Dto.Employee;

public class EmployeeSummaryDto
{
    public int TotalEmployee { get; set; }
    public int NSSPersonnel { get; set; }
    public int FullTime { get; set; }
    public int PartTime { get; set; }
    public int Others { get; set; } 
    public int Interns { get; set; }
    public int ActiveEmployee {get; set; }
    public int InActiveEmployee { get; set; }
}