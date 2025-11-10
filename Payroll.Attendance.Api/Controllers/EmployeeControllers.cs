using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Application.Services;
using Payroll.Attendance.Domain.Models;

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
            return Ok(result);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id, CancellationToken cancellationToken)
        {
            var employee = await service.GetEmployeeByIdAsync(id, cancellationToken);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");
            return Ok(employee);
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
       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updatedEmployee, CancellationToken cancellationToken)
        {
          var employee = await service.GetEmployeeByIdAsync(id, cancellationToken);
          if (employee == null)
            return NotFound($"Employee with ID {id} not found.");
          return Ok(employee);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken)
        {
            var existingEmployee = await service.GetEmployeeByIdAsync(id, cancellationToken);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            await service.DeleteEmployeeAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
        