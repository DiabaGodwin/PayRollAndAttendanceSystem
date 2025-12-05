namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class PayrollSummaryDto
{
    public decimal TotalBasicSalary { get; set; }
    public decimal TotalAllowance { get; set; }
    public decimal TotalDeduction { get; set; }
    public decimal NetPayroll { get; set; }

}