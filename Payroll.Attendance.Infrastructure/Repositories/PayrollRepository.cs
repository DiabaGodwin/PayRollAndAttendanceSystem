using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories
{
    public class PayrollRepository(ApplicationDbContext dbContext) : IPayrollRepository
    {

        public async Task<int> CreatePayrollAsync(PayrollRecord record, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(x=>x.Id == record.EmployeeId);
            if(employee == null)
                throw new Exception($"Employee not found");
          
            await dbContext.Payrolls.AddAsync(record, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return record.Id;
        }

        public async Task<List<PayrollRecord>> GetAllPayrollsAsync(PaginationRequest request,
            CancellationToken cancellationToken)
        {
            var query = dbContext.Payrolls .Include(a => a.Employee).AsQueryable();
        
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => 
                    x.Employee.FirstName.Contains(request.SearchText) ||
                    x.Employee.Surname.Contains(request.SearchText)
                );
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(x =>x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate);
            }

            query = query.OrderByDescending(X => X.EmployeeId);
          
        
            int skip = (request.PageNumber - 1) * request.PageSize;
                
            var res = await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return res; 
            
        }

        public async Task<PayrollRecord?> GetPayrollByIdAsync(int id, CancellationToken cancellationToken)
        {
            
           var result = await dbContext.Payrolls.Include(e => e.Employee)
               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
           return result;
        }

        public async Task<bool> UpdatePayrollAsync(UpdatePayrollRequest request, CancellationToken cancellationToken)
        {
            var exist = await dbContext.Payrolls.AnyAsync(x=>x.EmployeeId == request.EmployeeId, cancellationToken); 
            if(!exist) return false;
var result = await dbContext.Payrolls.Where(x => x.Employee.Id == request.EmployeeId).ExecuteUpdateAsync(y =>y
                .SetProperty(x => x.EmployeeId, request.EmployeeId)
                .SetProperty(x=>x.BasicSalary, request.BasicSalary)
                .SetProperty(x=>x.Allowance, request.Allowance)
                .SetProperty(x=>x.Deduction, request.Deduction)
                .SetProperty(x=>x.NetPay, request.NetPay)
                .SetProperty(x=>x.PayPeriod, request.PayPeriod)
                .SetProperty(x=>x.CreatedAt, DateTime.UtcNow)
                .SetProperty(x=>x.TotalDeduction, request.TotalDeduction)
                .SetProperty(x=>x.Loan, request.Loan)
                .SetProperty(x=>x.Tax, request.Tax)
                .SetProperty(x=>x.PayrollStatus, request.PayrollStatus)
                .SetProperty(x=>x.PaidDate, request.PaidDate)
               
                
            );
           
            return result > 0;
        }

        public async Task<bool> DeletePayrollAsync(int id, CancellationToken cancellationToken)
        {
           await dbContext.Payrolls.Where(x=>x.Id == id)
               .ExecuteUpdateAsync(x => x.SetProperty(x=>x.IsActive, false), cancellationToken);
           var result = await dbContext.SaveChangesAsync(cancellationToken);
           return result > 0;
        }

        public async Task<List<PayrollSummary>> GetAllPayrollSummaryAsync(CancellationToken cancellationToken)
        {
            var data = await dbContext.Payrolls.ToListAsync(cancellationToken);

            var summary = new PayrollSummary
            {
                TotalBasicSalary = (int)data.Sum(x => x.BasicSalary),
                TotalAllowance = (int)data.Sum(x => x.Allowance),
                TotalDeduction = (int)data.Sum(x => x.TotalDeduction),
                NetPayroll = (int)data.Sum(x => x.NetPay)
            };

            return new List<PayrollSummary> { summary };

        }
    }
} 