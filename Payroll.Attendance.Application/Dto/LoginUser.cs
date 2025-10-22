using System.ComponentModel.DataAnnotations;

namespace Payroll.Attendance.Application.Dto;

public class LoginUser
{

 public string UserName { get; set; } = string.Empty;
 
 public string Password { get; set; } = string.Empty;
}