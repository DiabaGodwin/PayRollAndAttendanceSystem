using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IPayrollRepository
    {
       Task<int> CreatePayrollAsync(PayrollRecord record, CancellationToken cancellationToken);
       
        Task<List<PayrollRecord>> GetAllPayrollsAsync(PaginationRequest request, CancellationToken cancellationToken);
        Task<PayrollRecord?> GetPayrollByIdAsync(int id, CancellationToken cancellationToken);
        
        Task<bool> UpdatePayrollAsync(UpdatePayrollRequest request, CancellationToken cancellationToken);
        Task<bool> DeletePayrollAsync(int id, CancellationToken cancellationToken);
        Task<List<PayrollSummary>> GetAllPayrollSummaryAsync(CancellationToken cancellationToken);
        Task<decimal> GetTotalPayrollAsync(CancellationToken cancellationToken);
        Task<int> GetHeadCountAsync(CancellationToken cancellationToken);
        Task<decimal> GetGrowthRateAsync(CancellationToken cancellationToken);
        Task<List<PayrollTrend>> GetMonthlyTrendAsync(CancellationToken cancellationToken);
        Task<List<DepartmentDistribution>> GetDepartmentDistributionAsync(CancellationToken cancellationToken);
        Task<List<PayrollSummaryDto>> GetPayrollSummaryByEmployeeIdAsync(int employeeId,
            CancellationToken cancellationToken); 
        
      
    }
}