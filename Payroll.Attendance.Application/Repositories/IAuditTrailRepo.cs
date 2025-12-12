using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;

public interface IAuditTrailRepo
{
    Task<int> SaveAuditTrail(AuditTrail auditTrail,CancellationToken ct);
    Task<IEnumerable<AuditTrail>> GetAllAuditTrails(DateTime from , DateTime to, CancellationToken ct);
}