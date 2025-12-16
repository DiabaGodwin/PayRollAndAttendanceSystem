using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;

public class CreateDepartmentDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
}