namespace Payroll.Attendance.Application.Dto.DashBoard;

public class ReminderSummaryDto
{
    public int TotalReminders { get; set; }
    public int Pending { get; set; }
    public int Overdue { get; set; }
    public int Completed { get; set; }
}

public class ReminderItemDto
{
    public int ReminderId { get; set; }
    public string? Title { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}