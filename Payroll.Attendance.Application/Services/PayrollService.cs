using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class PayrollService(IPayrollRepository repository, ILogger<PayrollService> logger) : IPayrollService
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
                    PayslipNumber = Guid.NewGuid().ToString()
                };
                var result = await repository.CreatePayrollAsync(payroll,cancellationToken);

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
                var result  = await repository.GetPayrollByIdAsync(dto.Id, cancellationToken);
                if (result == null)
                {
                    return new ApiResponse<GeneratePayslipDto>()
                    {
                        Message = "Payslipp record not found",
                        StatusCode = StatusCodes.Status404NotFound,
                    };
                }

                dto = result.Adapt(dto);
              

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
                return new ApiResponse<bool>()
                {
                    Message = "Payroll updated successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = result
                };
            return new ApiResponse<bool>()
            {
                Message = "Payroll failed to update",
                StatusCode = StatusCodes.Status500InternalServerError,
            };

        }

        public async Task<ApiResponse<List<PayrollResponseDto>>> DeletePayrollAsync(int id, CancellationToken cancellationToken)
        {
           var res = await repository.DeletePayrollAsync(id, cancellationToken);
           var response = res.Adapt(new List<PayrollResponseDto>());

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
    }
}