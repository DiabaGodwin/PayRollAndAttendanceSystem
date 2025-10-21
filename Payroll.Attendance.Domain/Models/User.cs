using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    public required string PasswordHash { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string SurName { get; set; } = null!;
}