using Microsoft.Extensions.DependencyInjection;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Application.Services;

namespace Payroll.Attendance.Application.ServiceExtensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IAttendanceCalculator, AttendanceCalculator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
    
}