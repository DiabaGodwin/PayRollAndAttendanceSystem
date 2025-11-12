using System.ComponentModel.DataAnnotations.Schema;
using Payroll.Attendance.Domain.Enum;


namespace Payroll.Attendance.Domain.Models;


public class AttendanceRecord
{
    public int Id { get; set; }
   
    public int EmployeeId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    public TimeSpan? TotalHours { get; set; }
    public bool IsLate { get; set; }
    public bool LeftEarly { get; set; }
    public string? Remarks { get; set; }
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    public string? CheckInLocation { get; set; }
    public string? CheckOutLocation { get; set; }
    public string? DeviceId { get; set; }
    public string? IPAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }

    [NotMapped]
    public double HoursWorked => CheckIn.HasValue && CheckOut.HasValue 
        ? (CheckOut.Value - CheckIn.Value).TotalHours 
        : 0;
}


