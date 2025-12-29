using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Application.Dto.DashBoard;
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

    public async Task<decimal> GetTotalPayrollAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Payrolls.SumAsync(f => f.NetPay);
    }

  

    public async Task<int> GetPendingReminderCountAsync(CancellationToken cancellationChangeToken)
    {
        return await dbContext.Reminders.CountAsync(e => e.IsCompleted);
    }

    public async Task<List<AttendanceTrendDto>> GetAttendanceTrendsAsync(DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken)
    {
       return await dbContext.Attendances.Where(s=>s.Date >= startDate && s.Date <= endDate)
           .GroupBy(s=> s.Date.Year).Select(s=>new AttendanceTrendDto
           {
               label = s.Key.ToString(),
               Present = s.Count(x=>x.Status == AttendanceStatus.Present),
               Absent = s.Count(x => x.Status == AttendanceStatus.Absent),
               LateArrivals = s.Count(x=>x.IsLate),
               TotalPenalties = s.Count(x=>x.IsLate) + s.Count(x=>x.Status == AttendanceStatus.Absent) + s.Count(x=>x.CheckIn==null)
           })
           .OrderBy(x => x.label)
           .ToListAsync(cancellationToken);
    }

    public async Task<List<DepartmentDistribution>> GetDepartmentDistributionsAsync(CancellationToken cancellationToken)
    {
        var result = await dbContext.Employees.GroupBy(e =>  e.Department!.Name ?? "Unkown")
            .Select(f => new DepartmentDistribution
            {
                DepartmentName = f.Key,
                Count = f.Count()
            }).ToListAsync(cancellationToken);
        return result;
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

    public async Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Attendances.Include(f => f.Employee).OrderByDescending(f => f.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PayrollRecord>> GetAllPayrollAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Payrolls.Include(f => f.Employee).OrderByDescending(f => f.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Departments.Include(x=>x.Employees).OrderByDescending(x=>x.CreatedAt).ToListAsync(cancellationToken);
    }
}