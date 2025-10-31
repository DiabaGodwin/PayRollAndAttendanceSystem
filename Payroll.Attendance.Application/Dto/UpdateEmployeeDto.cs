using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Dto;

public class UpdateEmployeeDto
{
    public int? Id { get; set; } 
    public string? Title { get; set; }
    public string? FirstName { get; set; } 
    public string? Surname { get; set; } 
    public string? OtherName { get; set; } 
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; } 
    public string? Address { get; set; }
    public Department? Department { get; set; }
    public string? JobPosition { get; set; }
    public string EmploymentType { get; set; } 
    public string? ReportingManager { get; set; }
    public string? Salary { get; set; }
    public string PayFrequency { get; set; } 
    public string EmploymentStatus  { get; set; } 
    public string? Category { get; set; }
}