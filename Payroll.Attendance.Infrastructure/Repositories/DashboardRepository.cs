using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Configurations;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class DashboardRepository(ApplicationDbContext dbContext) : IDashboardRepository
{
    public async Task<int> GetTotalEmployeesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Employees.CountAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalPayrollAsync(CancellationChangeToken cancellationChangeToken)
    {
        return await dbContext.Payrolls.SumAsync(f => f.NetPay);
    }

    public async Task<int> GetpresentCountAsync( CancellationToken cancellationToken)
    {
        return await dbContext.Attendances.CountAsync(d => d.Status == AttendanceStatus.Present, cancellationToken);
    }
    

    public async Task<int> GetPendingReminderCountAsync(CancellationChangeToken cancellationChangeToken)
    {
        return await dbContext.Reminders.CountAsync(e => e.IsCompleted);
    }

    public async Task<List<PayrollTrend>> GetPayrollTrendsAsync(int month,  CancellationChangeToken cancellationChangeToken)
    {
        var query = dbContext.Payrolls.AsQueryable();
       
        if (month > 0)
        {
            query = query.Where(e=>e.PayPeriod.Month == month);
        }

        return await query.GroupBy(a => a.PayPeriod.Month).Select(g => new PayrollTrend
        {
            Month = g.Key,
            Amount = g.Sum(e => e.NetPay),
            
        }).ToListAsync();
    }

    public async Task<List<DepartmentDistribution>> GetDepartmentDistributionsAsync(string department, CancellationChangeToken cancellationChangeToken)
    {
       var query = dbContext.Employees.AsQueryable();
       if (!string.IsNullOrEmpty(department))
       {
           query = query.Where(e => e.Department.Name == department);
       }

       return await query.GroupBy(e => e.Department).Select(g => new DepartmentDistribution
       {
           DepartmentName = g.Key,
           Count = g.Count()
       }).ToListAsync<DepartmentDistribution>();
    }

    public async Task<List<Reminder>> GetPendingRemindersAsync(CancellationToken cancellatioToken)
    {
        return await dbContext.Reminders.Where(r=>r.IsCompleted)
            .OrderBy(r=>r.Priority).ToListAsync();
    }

    public async Task<Reminder> AddReminderAsync(Reminder reminder, CancellationToken cancellationToken)
    {
        dbContext.Add(reminder);
        await dbContext.SaveChangesAsync(cancellationToken);
        return reminder;
    }

    public async Task<bool> DeleteReminderAsync(int id, CancellationToken cancellationToken)
    {
        await dbContext.Attendances.Where(x => x.Id == id).ExecuteUpdateAsync(x =>
            x.SetProperty(f => f.IsActive, false), cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;


    }
}