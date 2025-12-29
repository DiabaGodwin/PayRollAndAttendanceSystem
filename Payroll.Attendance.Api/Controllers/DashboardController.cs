using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Dto.DashBoard;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]

public class DashboardController(IDashboardService service) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        var result = await service.GetDashboardSummaryAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("payroll-trend")]
    public async Task<IActionResult> GetPayrollTrend(DateTime startDate, DateTime endDate,  CancellationToken cancellationToken)
    {
        var result = await service.GetAttendanceTrendAsync(startDate, endDate, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("department-distribution")]
    public async Task<IActionResult> GetDepartmentDistribuyion(
        CancellationToken cancellationToken)
    {
        var result = await service.GetDepartmentDistributionAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
        
    }

    [HttpPost("reminders")]
    public async Task<IActionResult> CreateReminders([FromBody] CreateReminderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.CreatReminderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete(("reminders/{id:int}"))]
    public async Task<IActionResult> DeleteReminder([FromBody] int id, CancellationToken cancellationToken)
    {
        var  result = await service.DeleteReminderAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
    /*

    [HttpGet("full")]
    public async Task<IActionResult> GetFullDashboard( CancellationToken cancellationToken)
    {
        var result = await service.GetFullDashboardAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
    */

    [HttpGet("recent/activites")]
    public async Task<IActionResult> GetRecentActivities(CancellationToken cancellationToken)
    {
        var result = await service.GetActivityAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
    
    
    
    

}