using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AuthRepository(ApplicationDbContext dbContext) : IAuthRepository
{
    public async Task AddUser(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users.FirstOrDefaultAsync(x=>x.Email==email,cancellationToken);
        return result ?? null;
    }

    public async Task<User?> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName==username,cancellationToken);
        return result;
    }

    public async Task<User?> GetUserByUserById(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return result;
    }

    public async Task<User?> CheckIfUserExists(string username, string email, CancellationToken cancellationToken)
        => await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email, cancellationToken);

    public async Task<User?> LoginUser(string usernameOrEmail, string requestPassword, CancellationToken cancellationToken)
         => await dbContext.Users.FirstOrDefaultAsync(x => (x.UserName == usernameOrEmail || x.Email == usernameOrEmail) && x.PasswordHash==requestPassword, cancellationToken);
    

    public async Task<bool> SaveRefreshToken(string refreshToken, DateTime tokenExpires, int userId, CancellationToken cancellationToken)
    {
         var result = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
         if(result == null) return false;
         result.RefreshExpires = tokenExpires;
         result.RefreshToken = refreshToken;
         var res = await dbContext.SaveChangesAsync(cancellationToken);
         return res > 0;
    }

    public async Task<User?> CheckIfUserWithTokenExist(string refresfToken, string userNameOrEmail, CancellationToken cancellationToken)
    {
        var res = await dbContext.Users.FirstOrDefaultAsync(x=> x.RefreshToken==refresfToken && (x.Email == userNameOrEmail || x.UserName==userNameOrEmail) && x.RefreshExpires > DateTime.UtcNow, cancellationToken);
        return res;
    }


    //Add a new method to save the refreshtoken and expiration

    public async Task<User?> LoginUser(User user, CancellationToken cancellationToken) =>
        await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName && x.PasswordHash == user.PasswordHash,
            cancellationToken);

    
}