namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class PayrollResponseDto
{
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    public Domain.Models.Employee? Employee { get; set; }
    public DateTime Payperiod { get; set; }
        
    
    public decimal BasicSalary { get; set; }
    
    public decimal Allowance { get; set; }
   
    public decimal Tax { get; set; }
   
    public decimal Loan { get; set; }
  
    public decimal Deduction { get; set; }  
        
        
    public decimal TotalDeduction { get; set; }
    public decimal NetPay { get; set; }
        
    public string? PayrollStatus { get; set; } 
   
    public DateTime PaidDate { get; set; }
        
    public string? PayslipPath { get; set; }
    public string? PayslipNumber { get; set; }
  

}