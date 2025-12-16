using Microsoft.EntityFrameworkCore.Storage;
using Payroll.Attendance.Application.Services;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task BeginTransactionAsync(CancellationToken ct=default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct=default)
    {
        try
        {
            await _context.SaveChangesAsync(ct);
            if (_transaction != null)
                await _transaction.CommitAsync(ct);
        }
        catch
        {
            await RollbackAsync(ct);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken ct=default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(ct);
        await DisposeTransactionAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct=default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
}