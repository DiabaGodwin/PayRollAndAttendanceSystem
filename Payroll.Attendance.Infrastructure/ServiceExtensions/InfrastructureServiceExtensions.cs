using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Application.Utility;
using Payroll.Attendance.Infrastructure.Data;
using Payroll.Attendance.Infrastructure.Repositories;

namespace Payroll.Attendance.Infrastructure.ServiceExtensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        services.AddScoped<ICryptographyUtility,  CryptographyUtility>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
    
    
}
