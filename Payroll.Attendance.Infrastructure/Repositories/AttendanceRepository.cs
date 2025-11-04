using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AttendanceRepository(ApplicationDbContext dbContext) : IAttendanceRepository
{
    public async Task<IEnumerable<AttendanceRecord>> GetAllAsync(CancellationToken cancellationToken)
    {
       var result = await dbContext.Attendances.ToListAsync(cancellationToken);
       return result;
    }

    public async Task<IEnumerable<AttendanceRecord>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Attendances.AsQueryable();

       
        if (request.EmployeeId.HasValue)
            query = query.Where(a => a.EmployeeId == request.EmployeeId.Value);

      
        if (request.StartDate.HasValue)
            query = query.Where(a => a.Date >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(a => a.Date <= request.EndDate.Value);

      
        int skip = (request.PageNumber - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<AttendanceRecord> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Attendances.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return result;
    }

    public async Task<int> CheckIn(AttendanceRecord attendanceRecord, CancellationToken cancellationToken)
    {
        var result = await dbContext.Attendances.AddAsync(attendanceRecord, cancellationToken);
        var res = await dbContext.SaveChangesAsync(cancellationToken);
        return res;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Attendances.FindAsync(id, cancellationToken);
        if (result is null) return;
        dbContext.Attendances.Remove(result);
        await dbContext.SaveChangesAsync(cancellationToken);
        
    }

    public async Task<int> CheckOut(int employeeId, CancellationToken cancellationToken)
    {
        var result = await dbContext.Attendances.FirstOrDefaultAsync(x=> x.EmployeeId==employeeId && x.CreatedAt == DateTime.Today,cancellationToken);
        if (result is null) return 0;
        result.CheckOut = DateTime.UtcNow;
        dbContext.Attendances.Update(result);
        await dbContext.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
}