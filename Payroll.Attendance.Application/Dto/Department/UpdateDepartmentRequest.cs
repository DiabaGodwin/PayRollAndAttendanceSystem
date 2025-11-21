using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Payroll.Attendance.Application.Dto.Department;

public class UpdateDepartmentRequest
{
    [Required]
    public int Id { get; set; }
    
   
    public string Name { get; set; }
    
   
    public string? Description { get; set; }
}