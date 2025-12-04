namespace Payroll.Attendance.Application.Dto.DashBoard;

public class CreateReminderRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}