using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Configurations;

namespace Payroll.Attendance.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public virtual DbSet<Profile> Profiles {get;set;}
    public virtual DbSet<User> Users {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.ApplyConfiguration(UserConfiguration);
    }
    
}