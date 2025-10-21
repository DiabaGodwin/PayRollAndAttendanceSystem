using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;

public class AddUserRequest
{   
    [Required]
    public string UserName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
}