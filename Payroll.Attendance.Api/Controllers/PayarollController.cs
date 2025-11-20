using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.PayrollDto;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController(IPayrollService service) : ControllerBase
    {
        [HttpGet("payroll")]
        public async Task<IActionResult> GetAllPayrolls([FromQuery] PaginationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await service.GetAllPayrollsAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("payroll/{id}")]
        public async Task<IActionResult> GetPayrollById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await service.GetPayrollByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Payroll summary")]
        public async Task<IActionResult> GetPayrollSummar(CancellationToken cancellationToken)
        {
            var result = await service.GetPayrollSummaryAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
            
        }

        [HttpPost("payroll")]
        public async Task<IActionResult> CreatePayroll([FromBody] CreatePayrollDto dto,
            CancellationToken cancellationToken)
        {
            var result = await service.CreatePayrollAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("payslip")]
        public async Task<IActionResult> GeneratePayslip([FromBody] GeneratePayslipDto dto,CancellationToken cancellationToken)

        {
            var result = await service.GeneratePayslipAsync(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
            
        }

        [HttpPut("payroll/{id}")]
        public async Task<IActionResult> UpdatePayroll( int id, [FromBody] UpdatePayrollRequest request,
            CancellationToken cancellationToken)
        {
            var result = await service.UpdatePayrollAsync(id, request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("payroll/{id}")]
        public async Task<IActionResult> DeletePayroll(int id, CancellationToken cancellationToken)
        {
            var result = await service.DeletePayrollAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        
 
        


    }
}