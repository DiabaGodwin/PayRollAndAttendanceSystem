using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;
public interface IDashboardRepository
{
    Task<int> GetTotalEmployeesAsync(CancellationToken cancellationToken);
    Task<decimal> GetTotalPayrollAsync(CancellationChangeToken cancellationChangeToken);
    Task<int> GetpresentCountAsync(CancellationToken cancellationToken);
    Task<int> GetPendingReminderCountAsync(CancellationChangeToken cancellationChangeToken);
    Task<List<PayrollTrend>> GetPayrollTrendsAsync(int month, CancellationChangeToken cancellationChangeToken);
    Task<List<DepartmentDistribution>> GetDepartmentDistributionsAsync(string department, CancellationChangeToken cancellationChangeToken);
    Task<List<Reminder>> GetPendingRemindersAsync(CancellationToken cancellatioToken);
    Task<Reminder> AddReminderAsync(Reminder reminder, CancellationToken  cancellationToken );
    Task<bool> DeleteReminderAsync(int id, CancellationToken cancellationToken);
    
    
}