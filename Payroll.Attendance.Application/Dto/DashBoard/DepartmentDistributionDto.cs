namespace Payroll.Attendance.Application.Dto.DashBoard;

public class DepartmentDistributionDto
{
    public string DepartmentName { get; set; }
    public string? SalaryAmount { get; set; }
    public decimal Percentage { get; set; }
    public decimal TotalAmount { get; set; }
    public int Count { get; set; }

}