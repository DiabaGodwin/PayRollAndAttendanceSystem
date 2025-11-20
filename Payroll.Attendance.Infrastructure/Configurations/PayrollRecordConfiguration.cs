using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Infrastructure.Configurations;

public class PayrollRecordConfiguration  : IEntityTypeConfiguration<PayrollRecord>
{
    public void Configure(EntityTypeBuilder<PayrollRecord> builder)
    {
        builder.HasKey(pr => pr.Id);
        builder.Property(Pr=> Pr.BasicSalary).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(pr=> pr.Allowance).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(pr => pr.Tax).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(pr=> pr.Deduction).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(pr=>pr.PayrollStatus).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(pr=>pr.CreatedAt).IsRequired().HasColumnType("datetime");
        


    }
}