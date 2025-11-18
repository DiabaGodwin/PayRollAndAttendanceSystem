namespace Payroll.Attendance.Application.Dto.PayrollRecordDto;

public class CreatePayrollDto
{
   
    public int EmployeeId { get; set; }
    public Domain.Models.Employee? Employee { get; set; }
    public DateTime Payperiod { get; set; }
        
   
    public decimal BasicSalary { get; set; }
    
    public decimal Allowance { get; set; }
 
    public decimal Tax { get; set; }

    public decimal Loan { get; set; }
   
    public decimal Deduction { get; set; }  
        
    

}