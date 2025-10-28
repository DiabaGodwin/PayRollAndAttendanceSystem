using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    
    public DbSet<Profile> Profiles {get;set;}
    public DbSet<User> Users {get;set;}
    
    public DbSet<Domain.Models.Attendance> AttendanceRecords {get;set;}

    public DbSet<Employee> Employees { get; set; }
    
    public DbSet<PayrollRecord> Payroll {get;set;}
    




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.ApplyConfiguration(UserConfiguration);
    }
    
}