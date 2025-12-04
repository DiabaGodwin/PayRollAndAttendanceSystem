using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Configurations;

namespace Payroll.Attendance.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    
    public DbSet<Profile> Profiles {get;set;}
    public DbSet<User> Users {get;set;}
    
    public DbSet<AttendanceRecord> Attendances {get;set;}

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    
    public DbSet<PayrollRecord> Payrolls {get;set;}
    public DbSet<Activity> Activities {get;set;}
    public DbSet<Reminder> Reminders {get;set;}
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AttendanceRecordConfiguration());
    }
}