using Microsoft.AspNetCore.Http;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Application.Utility;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AuthService(IAuthRepository repository, ICryptographyUtility cryptoUtility) : IAuthService
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
                IsSuccess = true,
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
            
            var hashPassword = cryptoUtility.HashPassword(request.Password);
            // Find user by username or email
            var user = await repository.CheckIfUserExists(request.UserNameOrEmail,hashPassword, cancellationToken);
            if (user == null) return new ApiResponse<LoginResponse>()
            {
                Message = @"Wrong username or password."
            };

            var verify = cryptoUtility.VerifyPassword(request.Password, hashPassword);
            if (!verify) return null;
            // Verify password
            if (!VerifyPassword(request.Password, user.PasswordHash))
                return null;

            
            return new ApiResponse<LoginResponse>();
        }
        catch (System.Exception)
        {
            return null;
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