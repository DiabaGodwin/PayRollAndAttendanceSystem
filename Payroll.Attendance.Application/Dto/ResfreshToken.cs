namespace Payroll.Attendance.Application.Dto;

public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshExpires { get; set; } 
}