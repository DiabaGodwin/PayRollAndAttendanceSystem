using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Services;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeDto addEmployeeDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.AddEmployeeAsync(addEmployeeDto, cancellationToken);

                return CreatedAtAction(nameof(GetEmployeeById), new { id = result},
                    new ApiResponse<int>
                    {
                        Message = " Employee Add successfully",
                        StatusCode = StatusCodes.Status201Created,
                        Data = 0
                    }
                
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>

                {
                    Message = $"Error Adding employee; {ex.Message}",
                    StatusCode = StatusCodes.Status400BadRequest,

                });

            }
        }

     
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(EmployeeResponseDto cancellationToken)
        {
            var result = await service.GetAllEmployeesAsync(cancellationToken);
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

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee updatedEmployee, CancellationToken cancellationToken)
        {
            var existingEmployee = await service.GetEmployeeByIdAsync(id, cancellationToken);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.Surname = updatedEmployee.Surname;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Department = updatedEmployee.Department;

            await service.UpdateEmployeeAsync(existingEmployee, cancellationToken);
            return Ok(existingEmployee);
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
        