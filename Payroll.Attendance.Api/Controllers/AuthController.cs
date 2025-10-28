using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Services;
using LoginRequest = Payroll.Attendance.Application.Dto.LoginRequest;

namespace Payroll.Attendance.Api;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AddUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.AddUser(request,cancellationToken);
        return Ok(result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login<LoginUser>([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result  = await service.LoginUser(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("UpdateUserRequest")]

    public async Task<ActionResult> UpdateUserRequest([FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateUserRequest(request, cancellationToken);
        return Ok(result);
    } 
    
}
