using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AuditTrailRepo(ApplicationDbContext contex) : IAuditTrailRepo
{
    public async Task<int> SaveAuditTrail(AuditTrail auditTrail, CancellationToken ct)
    {
        await contex.AuditTrails.AddAsync(auditTrail, ct);
        return await contex.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<AuditTrail>> GetAllAuditTrails(DateTime from, DateTime to, CancellationToken ct)
    {
       return await contex.AuditTrails.Include(d=>d.CreatedByUser)
           .Include(d=>d.UpdatedByUser).Where(r=>r.CreatedAt >= from && r.CreatedAt <= to)
           .ToListAsync(ct);
    }
}