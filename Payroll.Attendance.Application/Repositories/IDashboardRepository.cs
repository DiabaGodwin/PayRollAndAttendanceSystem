using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;
public interface IDashboardRepository
{
    Task<int> GetTotalEmployeesAsync(CancellationToken cancellationToken);
    Task<decimal> GetTotalPayrollAsync(CancellationToken cancellationToken);
    Task<int> GetpresentCountAsync(CancellationToken cancellationToken);
    Task<int> GetPendingReminderCountAsync(CancellationToken cancellationChangeToken);
    Task<List<PayrollTrend>> GetPayrollTrendsAsync( CancellationToken cancellationToken);
    Task<List<DepartmentDistribution>> GetDepartmentDistributionsAsync(
        CancellationToken cancellationToken);
    Task<List<Reminder>> GetPendingRemindersAsync(CancellationToken cancellatioToken);
    Task<Reminder> AddReminderAsync(Reminder reminder, CancellationToken  cancellationToken );
    Task<bool> DeleteReminderAsync(int id, CancellationToken cancellationToken);
    
    
}