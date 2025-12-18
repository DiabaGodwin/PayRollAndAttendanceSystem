using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class PayrollService(IPayrollRepository repository,
        ILogger<PayrollService> logger, IAuditTrailRepo auditTrailRepo,
        ICurrentUserService currentUserService,  IUnitOfWork unitOfWork) : IPayrollService
    {
        public async Task<ApiResponse<int>> CreatePayrollAsync(CreatePayrollDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var totalDeduction = dto.Tax + dto.Loan + dto.Deduction;
                var netPay = dto.BasicSalary  + dto.Allowance - totalDeduction;

                var payroll = new PayrollRecord
                {
                    EmployeeId = dto.EmployeeId,
                    PayPeriod = dto.PayPeriod,
                    BasicSalary = dto.BasicSalary,
                    Allowance = dto.Allowance,
                    Tax = dto.Tax,
                    Deduction = dto.Deduction,
                    Loan = dto.Loan,
                    TotalDeduction = totalDeduction,
                    NetPay = netPay,
                    PayrollStatus = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PayslipNumber = Guid.NewGuid().ToString(),
                    PayslipPath = $"/payslips/{dto.PayslipNumber}.pdf"
                };
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var result = await repository.CreatePayrollAsync(payroll,cancellationToken);
                var audit = await auditTrailRepo.SaveAuditTrail(new AuditTrail()
                {
                    Action = "PaysrollCreated",
                    Descriptions = $"Employee payslip generated",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserService.UserId,
                }, cancellationToken);
                if (result < 0 && audit < 0)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);
                }

                await unitOfWork.CommitAsync(cancellationToken);
                return new ApiResponse<int>()
                {
                    Data = result,
                    Message = "Payroll created successfully",
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (System.Exception var)
            {
                logger.LogError( "Error  generating payroll for EmployeeId {EmployeeId}", dto );
                await unitOfWork.RollbackAsync(cancellationToken);
                return new ApiResponse<int>()
                {
                    Message = "Creating payroll failed",
                    StatusCode = StatusCodes.Status400BadRequest,


                };


            }
        }
        
        //Generate Payslip

        public async Task<ApiResponse<GeneratePayslipDto>> GeneratePayslipAsync(GeneratePayslipDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var result  = await repository.GetPayrollByIdAsync(dto.Id, cancellationToken);
                if (result == null)
                {
                    var audit = await auditTrailRepo.SaveAuditTrail (new AuditTrail
                    {
                        Action = "PayslipGenerated",
                        Descriptions = $"Employee payslip generated",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = currentUserService.UserId,
                    }, cancellationToken);
                    
                    
                    
                    return new ApiResponse<GeneratePayslipDto>()
                    {
                        Message = "Payslipp record not found",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                }

                dto = result.Adapt<GeneratePayslipDto>();
                
                dto.TotalDeduction = dto.Tax + dto.Loan + dto.Deduction;
                dto.NetPay = dto.BasicSalary + dto.Allowance - dto.TotalDeduction;
                
                dto.PayrollStatus = "Pending";
                dto.UpdatedAt = DateTime.UtcNow;
                dto.PayslipNumber = Guid.NewGuid().ToString();
                dto.PayslipPath = $"/payslips/{dto.PayslipNumber}.pdf";
                dto.PaidDate = DateTime.UtcNow;
                
                await unitOfWork.CommitAsync(cancellationToken);
                return new ApiResponse<GeneratePayslipDto>()
                {
                    Message = "Payslip generated successfully",
                    StatusCode = StatusCodes.Status201Created,
                    Data = dto
                };

            }
            catch (System.Exception e)
            {
                logger.LogError( "Error  generating payroll for EmployeeId {EmployeeId}", dto.Id );
                await unitOfWork.RollbackAsync(cancellationToken);
                return new ApiResponse<GeneratePayslipDto>()
                {
                    Message = "Failed to generate payslip",
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse<List<PayrollResponseDto>>> GetAllPayrollsAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
           var res = await repository.GetAllPayrollsAsync(request, cancellationToken);
           var response = res.Adapt(new List<PayrollResponseDto>());
           
           return new ApiResponse<List<PayrollResponseDto>>()
           {
               Message = "Your request was successfully retrieved",
               StatusCode = StatusCodes.Status200OK,
               Data = response
           };
           
        }

        public async Task<ApiResponse<PayrollResponseDto>> GetPayrollByIdAsync(int id, CancellationToken cancellationToken)
        {
            var res = await  repository.GetPayrollByIdAsync(id, cancellationToken);
            var response = res.Adapt(new PayrollResponseDto());

            return new ApiResponse<PayrollResponseDto>()
            {
                Message = "Your request was successfully retrieved",
                StatusCode = StatusCodes.Status200OK,
                Data = response
            };
        }

        public async Task<ApiResponse<bool>> UpdatePayrollAsync(int id, UpdatePayrollRequest request, CancellationToken cancellationToken)
        {
            var result = await repository.UpdatePayrollAsync( request, cancellationToken);
            if (result)
            {
                var audit = await auditTrailRepo.SaveAuditTrail( new AuditTrail
                {
                    Action = nameof(UpdatePayrollAsync),
                    Descriptions = $"Payroll updated successfully with ID {id}",
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = currentUserService.UserId, 
                }, cancellationToken);;
                if(result && audit < 0)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);
                }
                
                await unitOfWork.CommitAsync(cancellationToken);
                return new ApiResponse<bool>()
                {
                    Message = "Payroll updated successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                };
                
            }
            await unitOfWork.CommitAsync(cancellationToken);    
            return new ApiResponse<bool>()
            {
                Message = "Payroll failed to update",
                StatusCode = StatusCodes.Status500InternalServerError,
            };

        }

        public async Task<ApiResponse<List<PayrollResponseDto>>> DeletePayrollAsync(int id, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
           var res = await repository.DeletePayrollAsync(id, cancellationToken);
           
           var audit = await auditTrailRepo.SaveAuditTrail( new AuditTrail
           {
               Action = "PaysrollDelete",
               Descriptions = $"Employee payslip deleted successfully",
               UpdatedAt = DateTime.UtcNow,
               UpdatedBy = currentUserService.UserId,
           }, cancellationToken);;
           if (res && audit < 0)
           {
               await unitOfWork.RollbackAsync(cancellationToken);
           }
           
           var response = res.Adapt(new List<PayrollResponseDto>());
           await unitOfWork.CommitAsync(cancellationToken);
           return new ApiResponse<List<PayrollResponseDto>>()
           {
               Message = "Payroll deleted successfully",
               StatusCode = StatusCodes.Status200OK,
               Data = response
           };
        }

       
        public async Task<ApiResponse<PayrollSummaryDto>> GetPayrollSummaryAsync( CancellationToken cancellationToken)
        {
            try
            {
                var payrolls = await repository.GetAllPayrollSummaryAsync( cancellationToken);
                var sum = payrolls.FirstOrDefault();
                if (sum == null)
                    return new ApiResponse<PayrollSummaryDto>()
                    {
                        Message = "Payroll summary not found",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                return new ApiResponse<PayrollSummaryDto>()
                {
                    Message = "Payroll summary retrieved successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = new PayrollSummaryDto()
                    {
                        TotalBasicSalary = sum.TotalBasicSalary,
                        TotalAllowance = sum.TotalAllowance,
                        TotalDeduction = sum.TotalDeduction,
                        NetPayroll = sum.NetPayroll
                    }
                };
                
            }
            
            catch (System.Exception e)
            {
                return new ApiResponse<PayrollSummaryDto>()
                {
                    Message = "Failed to retrieve payroll summary",
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<decimal> GetTotalPayrollAsync(CancellationToken cancellationToken)
        {
            return await repository.GetTotalPayrollAsync(cancellationToken);
        }

        public async Task<int> GetHeadCountAsync(CancellationToken cancellationToken)
        {
            return await repository.GetHeadCountAsync(cancellationToken);
        }

        public async Task<decimal> GetGrowthRateAsync(CancellationToken cancellationToken)
        {
            return await repository.GetGrowthRateAsync(cancellationToken);
        }

        public async Task<List<PayrollTrend>> GetMonthlyTrendAsync(CancellationToken cancellationToken)
        {
           return await repository.GetMonthlyTrendAsync(cancellationToken);
        }

        public async Task<List<DepartmentDistribution>> GetDepartmentSalaryDistributionAsync(CancellationToken cancellationToken)
        {
            return await repository.GetDepartmentDistributionAsync(cancellationToken);
        }
    }
}