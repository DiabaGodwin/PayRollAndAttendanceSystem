using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;

namespace Payroll.Attendance.Application.Services;

public interface IPayrollService
{
    Task<ApiResponse<int>> CreatePayrollAsync(CreatePayrollDto createPayrollDto, CancellationToken cancellationToken);
    Task<ApiResponse<GeneratePayslipDto>> GeneratePayslipAsync(GeneratePayslipDto generatePayslipDto, CancellationToken cancellationToken);
    Task<ApiResponse<List<PayrollResponseDto>>> GetAllPayrollsAsync(PaginationRequest request,CancellationToken cancellationToken);
    Task<ApiResponse<PayrollResponseDto>> GetPayrollByIdAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<bool>> UpdatePayrollAsync(int id, UpdatePayrollRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<List<PayrollResponseDto>>> DeletePayrollAsync(int id, CancellationToken cancellationToken);
    Task<ApiResponse<PayrollSummaryDto>> GetPayrollSummaryAsync( CancellationToken cancellationToken);
    
}