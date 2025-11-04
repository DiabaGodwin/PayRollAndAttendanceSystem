using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories
{
    public class PayrollRepository(ApplicationDbContext context) : IPayrollRepository
    {
        public Task AddPayrollAsync(PayrollRecord payroll, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<PayrollRecord>> GetPayrollsByEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PayrollRecord?> GetPayrollByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<PayrollRecord>> GetAllPayrollsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(PayrollRecord record)
        {
            throw new NotImplementedException();
        }
    }
}    