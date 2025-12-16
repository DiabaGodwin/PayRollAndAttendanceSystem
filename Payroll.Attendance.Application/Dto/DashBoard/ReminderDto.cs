namespace Payroll.Attendance.Application.Dto.DashBoard;

public class ReminderDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public DateTime DueDate { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    

}