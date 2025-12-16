namespace Payroll.Attendance.Application.Dto.DashBoard;

public class ActivityDto
{
    
    public int RecordId { get; set; }
    public int EmployeeId { get; set; }
    public string? Module {get; set; }
    public string? Description { get; set; }
    public string? EmployeeName { get; set; }
    public string? Action { get; set; }
    public DateTime Timestamp { get; set; }
 
}