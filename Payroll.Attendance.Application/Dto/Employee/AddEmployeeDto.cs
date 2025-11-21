using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto.Employee;

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
    
    public int DepartmentId { get; set; }
    public string? JobPosition { get; set; }

    [Required] public DateTime? HireDate { get; set; }
    [Required]
    public string? EmploymentType { get; set; } 
    [Range(0, double.MaxValue)]
    public string? Salary { get; set; }
    public string? PayFrequency { get; set; }
  

}