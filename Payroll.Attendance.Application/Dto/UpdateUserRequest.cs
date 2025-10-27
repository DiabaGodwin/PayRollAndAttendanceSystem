namespace Payroll.Attendance.Application.Dto;

public class UpdateUserRequest
{
  public int Id {get; set; }
  public string? FirstName {get;set;}
  public string? SurName { get; set; }
  public string? Password {get;set;}
  
}