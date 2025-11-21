using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto.Employee;

public class UpdateEmployeeRequest
{
  
    public string? Title { get; set; } = null!;
  
    public string? FirstName { get; set; } = null!;
   
    public string? Surname { get; set; } = null!;
   
    public string? OtherName { get; set; } = null!;
    
 
    public string? Email { get; set; } = null!;
   
  
    public string? PhoneNumber { get; set; } = null!;
  
    public DateTime? DateOfBirth { get; set; } 
 
    public string? Address { get; set; }
    
    public int? DepartmentId { get; set; }
    public string? JobPosition { get; set; }

     public DateTime? HireDate { get; set; }
   
    public string? EmploymentType { get; set; } 
    
    public string? Salary { get; set; }
    public string? PayFrequency { get; set; } 
    
}