using Payroll.Attendance.Application.Dto.PayrollDto;

namespace Payroll.Attendance.Application.Dto.Employee;

public class EmployeeAttendanceSummaryDto
{
  public DateTime Date { get; set; }
  public string? DayName { get; set; }
  public string? MonthName { get; set; }
  public int Present { get; set; }
  public int TotalPenalties { get; set; }
  public int Absent { get; set; }
  public int LateArrivals { get; set; }
  public int OnLeave { get; set; }
  
}

