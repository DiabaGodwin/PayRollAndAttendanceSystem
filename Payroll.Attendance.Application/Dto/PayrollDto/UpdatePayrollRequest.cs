namespace Payroll.Attendance.Application.Dto.PayrollDto;

public class UpdatePayrollRequest
{
    public int EmployeeId { get; set; }
   
    public DateTime PayPeriod { get; set; }
        
   
    public decimal BasicSalary { get; set; }
 
    public decimal Allowance { get; set; }
    
    public decimal Tax { get; set; }
 
    public decimal Loan { get; set; }
    
    public decimal Deduction { get; set; }  
        
        
    public decimal TotalDeduction { get; set; }
    public decimal NetPay { get; set; }
        
    public string? PayrollStatus { get; set; } 
  
    public DateTime PaidDate { get; set; }
  
        

}