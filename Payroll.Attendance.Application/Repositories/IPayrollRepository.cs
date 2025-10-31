using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IPayrollRepository
    {
        Task AddPayrollAsync(PayrollRecord payroll, CancellationToken cancellationToken);
        Task<List<PayrollRecord>> GetPayrollsByEmployeeAsync(int employeeId, CancellationToken cancellationToken);
        Task<PayrollRecord?> GetPayrollByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<PayrollRecord>> GetAllPayrollsAsync(CancellationToken cancellationToken);
        Task AddAsync(PayrollRecord record);
    }
}