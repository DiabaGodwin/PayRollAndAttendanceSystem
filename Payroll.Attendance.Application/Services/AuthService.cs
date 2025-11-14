using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Application.Utility;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AuthService(
    IAuthRepository repository, 
    ICryptographyUtility cryptoUtility,
    ILogger<AuthService> logger
    ) : IAuthService
{
    public async Task<ApiResponse<int>> AddUser(AddUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return new ApiResponse<int>()
                {
                    Message = "Email is required.",
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //Check if user exist
            var user = await repository.CheckIfUserExists(request.UserName,request.Email, cancellationToken);
            if (user != null)
            {
                return new ApiResponse<int>()
                {
                    Message = @"User already exists.",
                    StatusCode = StatusCodes.Status409Conflict,
                };
            }
            var passwordHash = cryptoUtility.HashPassword(request.Password);
            var newUser = new User()
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                UserName = request.UserName,
                FirstName = request.FirstName,
                SurName = request.SurName,
                
            };
            await repository.AddUser(newUser, cancellationToken);
            return new ApiResponse<int>()
            {
                Message = "User created.",
                Data = newUser.Id,
                StatusCode = StatusCodes.Status201Created,
            };
        }
        catch (System.Exception ex)
        {
            return new ApiResponse<int>()
            {
                Message = @"An error occured while creating user.",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        
    }
    
    
    public async Task<ApiResponse<LoginResponse>> LoginUser(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await repository.CheckIfUserExists(request.UserNameOrEmail,request.UserNameOrEmail, cancellationToken);
            if (user == null) return new ApiResponse<LoginResponse>()
            {
                Message = @"Wrong username or password.",
                StatusCode = StatusCodes.Status401Unauthorized,
                
            };
            var verify = cryptoUtility.VerifyPassword(request.Password, user.PasswordHash);
            if (!verify)
            {
                return new ApiResponse<LoginResponse>()
                {
                    Message = @"Wrong username or password.",
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
            }
            var userModel = user.Adapt(new UserModel());
            
            var token = cryptoUtility.GenerateToken(userModel);

            var response = new LoginResponse()
            {
                User = userModel,
                Token = token,
            };
            return new ApiResponse<LoginResponse>()
            { 
                Data = response,
                StatusCode = StatusCodes.Status200OK,
                Message = "Login successful."
            };
        }
        catch (System.Exception)
        {
            logger.LogError("An error occured while trying to login user.");
            return new ApiResponse<LoginResponse>()
            {
                Message = @"An error occured while trying to login user.",
                StatusCode = StatusCodes.Status500InternalServerError,

            };
        }
    }

    public Task<ApiResponse<int>> UpdateUserRequest(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private bool VerifyPassword(string requestPassword, string userPasswordHash)
    {
        throw new NotImplementedException();
    }
}