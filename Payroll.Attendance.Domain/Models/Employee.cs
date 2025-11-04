using System.ComponentModel.DataAnnotations;
using Payroll.Attendance.Domain.Enum;


namespace Payroll.Attendance.Domain.Models
{
    public class Employee
    {
   

        public int? Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string OtherName { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } 
        public string? Address { get; set; }
        public string Department { get; set; }
        public string JobPosition { get; set; }
        public DateTime? HireDate { get; set; }
        public EmploymentType EmploymentType { get; set; } 
        public int EmployeeId{ get; set; }
        public string ReportingManager { get; set; }
        public string? Salary { get; set; }
        public string PayFrequency { get; set; } = null!;
        public string EmploymentStatus  { get; set; } 
        public string Category { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
    }

    public class Department
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class EmployeeSummary
    {
        public int TotalEmployee { get; set; }
        public int NSSPersonnel { get; set; }
        public int interns { get; set; }
        public int ActiveEmployee {get; set; }
        public int InActiveEmployee { get; set; }
        
    }
}