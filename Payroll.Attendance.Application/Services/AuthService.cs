using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AuthService(IAuthRepository repository) : IAuthService
{
    public async Task<int> AddUser(AddUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return 0;
            }
            //Check if user exist
            var user = await repository.CheckIfUserExists(request.UserName,request.Email, cancellationToken);
            if(user != null) return 0;
            var passwordHash = HashPassword(request.Password);
            var newUser = new User()
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                UserName = request.UserName,
            };
            await repository.AddUser(newUser, cancellationToken);
            return newUser.Id;
        }
        catch (Exception ex)
        {
            return 0;
        }
        
    }

    private string HashPassword(string password)
    {
        //Hash password
        return password;
    }
}