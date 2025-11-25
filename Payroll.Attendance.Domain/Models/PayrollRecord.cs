using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Attendance.Domain.Models
{
    public class PayrollRecord
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey((nameof(Models.Employee)))]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime PayPeriod { get; set; }
       
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Loan { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deduction { get; set; } 
        
        public int Month { get; set; }
        public int Year { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }
        public string PayrollStatus { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        public DateTime PaidDate { get; set; }
        
        public string? PayslipPath { get; set; }
        public string? PayslipNumber { get; set; }
        
    }

    public class PayrollSummary
    {
        public int TotalBasicSalary { get; set; }
        public int TotalAllowance { get; set; }
        public int TotalDeduction { get; set; }
        public int NetPayroll { get; set; }
        
        
    }
}