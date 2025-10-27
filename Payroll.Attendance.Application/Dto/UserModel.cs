namespace Payroll.Attendance.Application.Dto;

public class UserModel
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string SurName { get; set; } = null!;
    public bool IsActive { get; set; }
}