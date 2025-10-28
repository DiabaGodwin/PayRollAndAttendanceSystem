namespace Payroll.Attendance.Application.Dto;

public class LoginResponse
{
    public UserModel? User { get; set; }
    public string AccessToken { get; set; } = null!;
    public DateTime Expires { get; set; }

}

