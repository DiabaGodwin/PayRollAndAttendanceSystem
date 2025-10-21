using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;

public interface IAuthRepository
{
    Task AddUser(User user,CancellationToken cancellationToken);
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<User> GetUserByUsername(string username, CancellationToken cancellationToken);
    Task<User> GetUserByUserById(int id, CancellationToken cancellationToken);
    Task<User?> CheckIfUserExists(string username,string email, CancellationToken cancellationToken);
}