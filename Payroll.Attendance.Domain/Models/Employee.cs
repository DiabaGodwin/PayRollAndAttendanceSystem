using System.ComponentModel.DataAnnotations;
using Payroll.Attendance.Domain.Enum;


namespace Payroll.Attendance.Domain.Models
{
    public class Employee
    {
       

        public int Id { get; set; } 
        public string Title { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? OtherName { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } 
        public string? Address { get; set; }
        
        
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        
        public string? JobPosition { get; set; }
        public DateTime? HireDate { get; set; }
        public string EmploymentType { get; set; } = null!; 
        public int EmployeeId{ get; set; }
        public string? ReportingManager { get; set; }
        public string? Salary { get; set; }
        public string?   PayFrequency { get; set; }
        
        public bool IsActive { get; set; } = true;
       
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
       
    }

    public class EmployeeSummary
    {
        public int TotalEmployee { get; set; }
        public int NssPersonnel { get; set; }
        public int FullTime { get; set; }
        public int PartTime { get; set; }
        public int Others {  get; set; }
        public int Interns { get; set; }
        public int ActiveEmployee {get; set; }
        public int InActiveEmployee { get; set; }
        
    }
}