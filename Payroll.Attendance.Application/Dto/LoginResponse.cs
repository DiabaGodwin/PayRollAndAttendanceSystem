namespace Payroll.Attendance.Application.Dto;

public class LoginResponse
{
    public UserModel? User { get; set; }
    public TokenResponse Token { get; set; } = new TokenResponse();
    
    public RefreshTokenDto RefreshToken { get; set; } = new RefreshTokenDto();
    
}

