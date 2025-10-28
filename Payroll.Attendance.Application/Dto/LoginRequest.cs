using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;

public class LoginRequest
{

 public required string UserNameOrEmail { get; set; }
 
 public required string Password { get; set; } 
}