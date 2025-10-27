using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IAuthService
{
    Task<int> AddUser(AddUserRequest request, CancellationToken cancellationToken);
    Task<User?> LoginUser(LoginUser request, CancellationToken cancellationToken);
    Task<User> UpdateUserRequest(UpdateUserRequest request, CancellationToken cancellationToken);
}