using System.ComponentModel.DataAnnotations;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto;

public class AddEmployeeDto
{ 
   
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    [MaxLength(55)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Surname { get; set; } = null!;
    [MaxLength(50)]
    public string OtherName { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    public DateTime? DateOfBirth { get; set; } 
    [Required]
    public string? Address { get; set; }
    
    public string? Department { get; set; }
    public string? JobPosition { get; set; }

    [Required] public DateTime? HireDate { get; set; }
    [Required]
    public EmploymentType EmploymentType { get; set; } 
    [Range(0, double.MaxValue)]
    public string? Salary { get; set; }
    public string PayFrequency { get; set; } = null!;
    
}