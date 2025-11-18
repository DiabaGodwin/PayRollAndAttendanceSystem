namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class PayrollSummaryDto
{
    public int TotalBasicSalary { get; set; }
    public int TotalAllowance { get; set; }
    public int TotalDeduction { get; set; }
    public int NetPayroll { get; set; }

}