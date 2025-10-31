namespace Payroll.Attendance.Application.Dto;


public class AttendanceSummaryDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int TotalDaysPresent { get; set; }
    public int TotalDaysAbsent { get; set; }
    public int TotalDaysLate { get; set; }
    public int TotalDaysOnLeave { get; set; }
    public double TotalHoursWorked { get; set; }
    public double AverageHoursPerDay { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}