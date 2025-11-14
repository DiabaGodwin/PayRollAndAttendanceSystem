using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AttendanceRepository(ApplicationDbContext dbContext) : IAttendanceRepository
{
    public async Task<List<AttendanceRecord>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Attendances .Include(a => a.Employee).AsQueryable();
        
        if (!string.IsNullOrEmpty(request.SearchText))
            query = query.Where(x => 
                x.Employee.FirstName.Contains(request.SearchText) ||
                x.Employee.Surname.Contains(request.SearchText)
            );
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
             query = query.Where(x =>x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate);
        }
        
        int skip = (request.PageNumber - 1) * request.PageSize;
                
        var data = await query
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return data; 
       
    }

    public async Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken)
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
        var today = DateTime.UtcNow.Date;
        var result = await dbContext.Attendances.FirstOrDefaultAsync(x=> x.EmployeeId==employeeId && x.Date == today,cancellationToken);
        if (result is null) return 0;
        result.CheckOut = DateTime.UtcNow;
        dbContext.Attendances.Update(result);
        await dbContext.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
    
    public async Task<IEnumerable<AttendanceRecord>> GetByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        return await dbContext.Attendances
            .Where(a => a.Date == date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Attendances.Include(x => x.Employee).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Employees.CountAsync((cancellationToken));
    }
}