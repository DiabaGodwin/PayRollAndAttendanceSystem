using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories
{
    public class PayrollRecordRepository(ApplicationDbContext context) : IPayrollRecordRepository
    {
       
        public async Task AddPayrollAsync(PayrollRecord payroll, CancellationToken cancellationToken)
        {
            await context.PayrollRecords.AddAsync(payroll, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
      
        public async Task<PayrollRecord?> GetPayrollByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.PayrollRecords
                .Include(p => p.Employee) 
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
        
        public async Task<List<PayrollRecord>> GetPayrollsByEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await context.PayrollRecords
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.PeriodEnd)
                .Include(p => p.Employee) 
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PayrollRecord>> GetAllPayrollsAsync(CancellationToken cancellationToken)
        {
            return await context.PayrollRecords
                .Include(p => p.Employee)
                .OrderByDescending(p => p.PeriodEnd)
                .ToListAsync(cancellationToken);
        }

        public Task AddAsync(PayrollRecord record)
        {
            throw new NotImplementedException();
        }
    }
}