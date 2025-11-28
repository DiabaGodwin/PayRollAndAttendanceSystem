namespace Payroll.Attendance.Application.Dto.DashBoard;

public class ActivityDto
{
    public int Id { get; set; }
    public int EmployeeId {get; set;}
    public string? EmployeeName { get; set; }
    public string? Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string? TimeDisplay { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }    

}