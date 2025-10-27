using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Services;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // ✅ Create new employee
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto, CancellationToken token)
        {
            var employee = new Employee(
                dto.FullName,
                dto.Category,
                dto.Department,
                dto.BasicSalary,
                dto.Allowance,
                dto.TopUp
            );

            await _employeeService.AddEmployeeAsync(employee, token);
            return Ok("Employee created successfully.");
        }

        // ✅ Get all employees
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployees(CancellationToken token)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(token);
            return Ok(employees);
        }

        // ✅ Get single employee by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id, CancellationToken token)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id, token);
            if (employee == null)
                return NotFound("Employee not found.");
            return Ok(employee);
        }
    }

    // DTO (Data Transfer Object)q
    public record CreateEmployeeDto(
        string FullName,
        EmployeeCategory Category,
        string Department,
        decimal BasicSalary,
        decimal Allowance,
        decimal TopUp
    );
}