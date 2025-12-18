using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto.Employee;

public class EmployeeAnalyticsDto
{
    public List<PayrollSummaryDto> PayrollSummary { get; set; } = new();
    public Domain.Models.Employee employeeResponse { get; set; } = new();
    public List<EmployeeAttendanceSummaryDto> EmployeeAttendanceSummary { get; set; } = new();
    public List<AttendanceTrendDto> AttendanceTrend { get; set; } = new();
    public ComplianceAndPerformanceDto? ComplianceAndPerformance { get; set; } = new();
}

public class AttendanceTrendDto
{
    public string label { get; set; }
    public int Present { get; set; }
    public int TotalPenalties { get; set; }
    public int Absent { get; set; }
    public int LateArrivals { get; set; }
}

public class ComplianceAndPerformanceDto
{
    public int LateCheckIn { get; set; }
    public int MissedCheckOut { get; set; }
    public int TotalPenalties {get; set;}
}