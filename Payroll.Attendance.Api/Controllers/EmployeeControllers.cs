using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpPost("employee")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDto request, CancellationToken cancellationToken)
        {
            var result = await service.AddEmployeeAsync(request,cancellationToken);
         return StatusCode(result.StatusCode,result);
        }

      

     
        [HttpGet("employee")]
        public async Task<IActionResult> GetAllEmployees([FromQuery]PaginationRequest request, CancellationToken cancellationToken)
        {
            var result = await service.GetAllEmployeesAsync(request,cancellationToken);
            return StatusCode(result.StatusCode,result);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetEmployeeByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode,result);
        }

        [HttpGet("employee/IdAndName")]
        public async Task<IActionResult> GetEmployeeIdAndName(string? searchText, CancellationToken cancellationToken)
        {
            var result = await service.GetEmployeeIdAndName(searchText, cancellationToken);
            return StatusCode(result.StatusCode,result);
 
        }

        [HttpGet("Employees/summary")]
        public async Task<IActionResult> GetEmployeeSummaryByAsync (CancellationToken cancellationToken)
        {
            var result = await service.GetEmployeeSummaryAsync(cancellationToken );
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("Employees/analytics/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAnalytics(int employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)  
        {
            var res = await service.GetEmployeeAnalyticsAsync(employeeId, startDate, endDate, cancellationToken);
            return StatusCode(res.StatusCode,res);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest updatedEmployee,
            CancellationToken cancellationToken)
        {
            var result = await service.UpdateEmployeeAsync(id,updatedEmployee,  cancellationToken);
            return StatusCode(result.StatusCode,result);
        }
        
            

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        { 
            var result  = await service.DeleteEmployeeAsync(id, cancellationToken);
            return StatusCode(result.StatusCode,result);
        }
    }
}
        