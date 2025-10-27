using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Services;

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
    
    



    }