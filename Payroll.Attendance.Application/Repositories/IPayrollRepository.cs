using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Application.Dto.PayrollRecordDto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IPayrollRepository
    {
       Task<PayrollRecord> CreatePayrollAsync(PayrollRecord record, CancellationToken cancellationToken);
       /*
       Task<PayrollRecord> GeneratePayrollRecordAsync(int EmployeeId, CancellationToken cancellationToken);
       */
        Task<List<PayrollRecord>> GetAllPayslipAsync(CancellationToken cancellationToken);
        Task<PayrollRecord> GetPayslipByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<bool> UpdatePayrollAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeletePayrollAsync(int id, CancellationToken cancellationToken);
        Task<List<PayrollSummary>> GetAllPayrollSummaryAsync(CancellationToken cancellationToken);
      
    }
}