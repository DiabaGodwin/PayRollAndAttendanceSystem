using Payroll.Attendance.Application.Dto.DashBoard;

namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class PayrollTrendSummaryDto
{
    public decimal CurrentMonthPayroll { get; set; }
    public decimal PreviousMonthPayroll { get; set; }
    public decimal YearToPayroll { get; set; }
    public decimal MonthOverMonthChangePercentage { get; set; }
    public List<PayrollTrendDto> MonthlyTrends { get; set; } = new();

}

public class PayrollTrendPointDto
{
    public string? Month { get; set; }
    public decimal TotalPaid { get; set; }
}