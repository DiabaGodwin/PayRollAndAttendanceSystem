using System.ComponentModel.DataAnnotations;
using Payroll.Attendance.Domain.Enum;

namespace Payroll.Attendance.Application.Dto.Employee;

public class EmployeeResponseDto
{
    public int? Id { get; set; } 
    public string? Title { get; set; }
    public string FirstName { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? OtherName { get; set; }
    public string FullName => $"{Title} {FirstName}  {Surname}  {OtherName}"; 
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; } 
    public string? Address { get; set; }
   
    public string? JobPosition { get; set; }
    public DateTime? HireDate { get; set; }
    public string? EmploymentType { get; set; } 
    public string? ReportingManager { get; set; }
    public string? Salary { get; set; }
    public string PayFrequency { get; set; } = null!;
    public string? EmploymentStatus  { get; set; } 
    public string? Department { get; set; }
  
   
}