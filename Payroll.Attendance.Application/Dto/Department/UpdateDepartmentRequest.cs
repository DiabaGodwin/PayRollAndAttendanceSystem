using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto.Department;

public class UpdateDepartmentRequest
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
}