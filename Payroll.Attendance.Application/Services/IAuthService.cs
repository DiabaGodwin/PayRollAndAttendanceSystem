using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IAuthService
{
    Task<ApiResponse<int>> AddUser(AddUserRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<LoginResponse>> LoginUser(LoginRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<int>> UpdateUserRequest(UpdateUserRequest request, CancellationToken cancellationToken);
}