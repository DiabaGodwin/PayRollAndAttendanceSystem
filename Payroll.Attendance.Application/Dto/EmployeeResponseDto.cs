using System.ComponentModel.DataAnnotations;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto;

public class EmployeeResponseDto
{
    public int? Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string OtherName { get; set; } = null!;
    public string FullName => $"{Title} {FirstName}  {Surname}  {OtherName}"; 
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; } 
    public string? Address { get; set; }
    public Department? Department { get; set; }
    public string? JobPosition { get; set; }
    public DateTime? HireDate { get; set; }
    public EmploymentType EmploymentType { get; set; } 
    public string? ReportingManager { get; set; }
    public string? Salary { get; set; }
    public string PayFrequency { get; set; } = null!;
    public string EmploymentStatus  { get; set; } 
    public string Category { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}