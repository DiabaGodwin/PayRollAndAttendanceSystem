using Payroll.Attendance.Domain.Enum;


namespace Payroll.Attendance.Domain.Models;


public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public int? LateMinutes { get; set; }
    public int? OvertimeMinutes { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual Employee Employee { get; set; } = null!;
}

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    HalfDay,
    Leave,
    Holiday
}