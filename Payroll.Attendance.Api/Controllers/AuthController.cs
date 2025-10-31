using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AddUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.AddUser(request,cancellationToken);
        return StatusCode(result.StatusCode,result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result  = await service.LoginUser(request, cancellationToken);
        return StatusCode(result.StatusCode,result);
    }

    [HttpPut("UpdateUserRequest")]

    public async Task<ActionResult> UpdateUserRequest([FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateUserRequest(request, cancellationToken);
        return StatusCode(result.StatusCode,result);
    } 
    
}
