using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Department;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService service) : ControllerBase
{
    [HttpPost("CreateDepartment")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto,
        CancellationToken cancellationToken)
    {
        var response = await service.CreateDepartmentAsync(createDepartmentDto, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("departments")]
    public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken = default)
    {
        var response = await service.GetAllOnlyDepartmentsAsync(cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("department/{id}")]
    public async Task<IActionResult> GetDepartmentById(int id, [FromRoute] bool includeEmployees = false,
        CancellationToken cancellationToken = default)
    {
        var response = await service.GetDepartmentByIdAsync(id, includeEmployees, cancellationToken);
            return StatusCode(response.StatusCode);
    }

    [HttpPut("updatedepartment")]
    public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentRequest updateDepartmentRequest,
        CancellationToken cancellationToken)
    {
        var response = await service.UpdateDepartmentAsync(updateDepartmentRequest, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("deletedepartment/id/{id}")]
    public async Task<IActionResult> DeleteDepartmentById(int id, CancellationToken cancellationToken)
    {
        var response = await service.DeleteDepartmentAsync(id, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    
}