using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Services;
using Payroll.Attendance.Domain.Models;


namespace Payroll.Attendance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController(IAttendanceService service) : ControllerBase

{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode,result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GeByIdt(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode,result);
        
    }  
    
 
    [HttpPost("CheckIn")]
    public async Task<IActionResult> Add([FromBody] AttendanceRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CheckIn(request, cancellationToken);
        return StatusCode(result.StatusCode,result);
    }
    
    [HttpPut("checkout")]
    public async Task<IActionResult> CheckOut([FromBody] UpdatedAttendanceRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CheckOutAsync(request, cancellationToken);
        return StatusCode(result.StatusCode,result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode,result);
    }
    
}