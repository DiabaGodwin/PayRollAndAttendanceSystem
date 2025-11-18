using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Domain.Models
{
    public class PayrollRecord
    {
        public int Id { get; set; }
        
        [Required]
        [Key]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime Payperiod { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal BasicSalary { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Allowance { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Tax { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Loan { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Deduction { get; set; }  
        
        
        public decimal TotalDeduction { get; set; }
        public decimal NetPay { get; set; }
        
        public string PayrollStatus { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } 
        public DateTime PaidDate { get; set; }
        
        public string PayslipPath { get; set; }
        public string PayslipNumber { get; set; }
        public string EmplooymentType { get; set; }



    }

    public class PayrollSummary
    {
        public int TotalBasicSalary { get; set; }
        public int TotalAllowance { get; set; }
        public int TotalDeduction { get; set; }
        public int NetPayroll { get; set; }
        
        
    }
}