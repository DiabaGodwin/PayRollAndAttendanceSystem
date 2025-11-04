using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x=>x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.UtcNow);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255).IsUnicode(false);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(255).IsUnicode(true);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(255).IsUnicode(true);
        
        

    }
}