using Microsoft.AspNetCore.Mvc;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _payrollService;

        public PayrollController(PayrollService payrollService)
        {
            _payrollService = payrollService;
        }

    }
}