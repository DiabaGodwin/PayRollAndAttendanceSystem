using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Payroll.Attendance.Application.Dto;

namespace Payroll.Attendance.Application.Services;

public interface ICurrentUserService
{
    int UserId { get; }
    string Email { get; }
    UserModel? UserAuthData { get; }
    Task<string?> GetBearerTokenAsync();
}

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{

    public int UserId
    {
        get
        {
            var res = httpContextAccessor.HttpContext?.User.FindFirst("custom:userId")?.Value;
            return int.TryParse(res, out var userId) ? userId : 0;
        }
    }

    public string Email => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value!;
    public UserModel? UserAuthData
    {
        get
        {
            var claimValue = httpContextAccessor.HttpContext?.User.FindFirst("custom:user_auth_data")?.Value;
            return string.IsNullOrWhiteSpace(claimValue) ? null : JsonSerializer.Deserialize<UserModel>(claimValue);
        }
    }

    public Guid UserProfileId
    {
        get
        {
            var res = httpContextAccessor.HttpContext?.User.FindFirst("custom:user_profile_id")?.Value;
            return Guid.TryParse(res, out var guid) ? guid : Guid.Empty;
        }
    }
    
    
    
    public async Task<string?> GetBearerTokenAsync()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null) return null;

        return await context.GetTokenAsync("access_token");
    }
    
}