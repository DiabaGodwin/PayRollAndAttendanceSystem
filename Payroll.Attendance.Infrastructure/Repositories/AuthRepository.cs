using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AuthRepository(ApplicationDbContext context) : IAuthRepository
{
    public async Task AddUser(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByUserById(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> CheckIfUserExists(string username, string email, CancellationToken cancellationToken)
     => await context.Users.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email, cancellationToken);
     
}