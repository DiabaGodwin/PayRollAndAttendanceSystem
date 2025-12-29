using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Application.Dto.DashBoard;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;
public interface IDashboardRepository
{
    Task<int> GetTotalEmployeesAsync(CancellationToken cancellationToken);
    Task<decimal> GetTotalPayrollAsync(CancellationToken cancellationToken);
    
    Task<int> GetPendingReminderCountAsync(CancellationToken cancellationChangeToken);
    Task<List<AttendanceTrendDto>> GetAttendanceTrendsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<List<DepartmentDistribution>> GetDepartmentDistributionsAsync(
        CancellationToken cancellationToken);
    Task<List<Reminder>> GetPendingRemindersAsync(CancellationToken cancellatioToken);
    Task<Reminder> AddReminderAsync(Reminder reminder, CancellationToken  cancellationToken );
    Task<bool> DeleteReminderAsync(int id, CancellationToken cancellationToken);
    Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken);
    Task<List<PayrollRecord>> GetAllPayrollAsync(CancellationToken cancellationToken);
    Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken);
    
    
}