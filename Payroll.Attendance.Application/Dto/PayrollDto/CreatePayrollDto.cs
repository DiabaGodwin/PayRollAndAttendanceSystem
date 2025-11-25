using Payroll.Attendance.Domain.Enum;

namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class CreatePayrollDto
{
   
    public int EmployeeId { get; set; }

    public DateTime PayPeriod { get; set; }
        
   
    public decimal BasicSalary { get; set; }
    
    public decimal Allowance { get; set; }
 
    public decimal Tax { get; set; }

    public decimal Loan { get; set; }
   
    public decimal Deduction { get; set; }
    public string PayrollStatus { get; set; } = "Pending";
    public string? PayslipPath { get; set; }
    public string? PayslipNumber { get; set; }
  



}