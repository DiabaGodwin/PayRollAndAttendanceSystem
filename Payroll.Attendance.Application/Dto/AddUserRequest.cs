using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;

public class AddUserRequest
{   
    [Required,MaxLength(50)]
    public string UserName { get; set; } = null!;
    
    [Required,MaxLength(50),DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    
    [Required,MinLength(6)] 
    public string Password { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string SurName { get; set; } = null!;
}