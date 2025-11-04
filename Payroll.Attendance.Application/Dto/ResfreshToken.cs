namespace Payroll.Attendance.Application.Dto;

public class RefreshToken
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; } 
}