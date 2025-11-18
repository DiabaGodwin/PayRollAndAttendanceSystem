using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories
{
    public class PayrollRepository(ApplicationDbContext dbContext) : IPayrollRepository
    {
        private IPayrollRepository _payrollRepositoryImplementation;

        public async Task<PayrollRecord> CreatePayrollAsync(PayrollRecord record, CancellationToken cancellationToken)
        {
            await dbContext.Payrolls.AddAsync(record, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return record;
        }

       
/*
        public Task<PayrollRecord> GeneratePayrollRecordAsync(int employeeId,  CancellationToken cancellationToken)
        {
            
            var result = dbContext.Payrolls.Include(x=>x.BasicSalary)
                .FirstOrDefault(x => x.Id == employeeId);
            if (result == null)
                throw new Exception("Active Employee not found");

            var existingPayrollRecord = dbContext.Payrolls.Include(x => x.BasicSalary);
            return 

        }
        */

        public Task<List<PayrollRecord>> GetAllPayslipAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PayrollRecord> GetPayslipByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePayrollAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePayrollAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<PayrollSummary>> GetAllPayrollSummaryAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}    