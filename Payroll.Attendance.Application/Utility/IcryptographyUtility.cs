using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Utility;

public interface ICryptographyUtility
{
    string HashPassword (string password);
    bool VerifyPassword(string password, string hashedPassword);
    TokenResponse GenerateToken(UserModel user);
    RefreshTokenDto GenerateRefreshToken();

}

