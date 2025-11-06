using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Domain.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
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
        
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } 
        public DateTime PaidDate { get; set; }
        
        public string PayslipPath { get; set; }
        public string PayslipNumber { get; set; }



    }
}