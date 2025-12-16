using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Infrastructure.Configurations;

public class DepartmentConfiguration :  IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000).HasMaxLength(500);
        builder.Property(x=>x.CreatedAt).HasDefaultValueSql("getUtcdate()");
        builder.HasMany(x => x.Employees).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Cascade);
    }
}