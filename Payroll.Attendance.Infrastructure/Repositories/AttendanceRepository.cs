using Microsoft.EntityFrameworkCore;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AttendanceRepository(ApplicationDbContext dbContext) : IAttendanceRepository
{
    private IAttendanceRepository _attendanceRepositoryImplementation;

    public async Task<List<AttendanceRecord>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Attendances .Include(a => a.Employee)
            .ThenInclude(x=>x.Department).AsQueryable();
        
        if (!string.IsNullOrEmpty(request.SearchText))
            query = query.Where(x => 
                x.Employee.FirstName.Contains(request.SearchText) ||
                x.Employee.Surname.Contains(request.SearchText) ||
                x.Employee.Department.Name.Contains(request.SearchText)
                
            );
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
             query = query.Where(x =>x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate);
        }


        query = query.OrderByDescending(x => x.Date).ThenByDescending(x=> x.CheckIn)
       .ThenByDescending(x=>x.CheckOut);
        
        int skip = (request.PageNumber - 1) * request.PageSize;
                
        var data = await query
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return data; 
       
    }

    public async Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Attendances.Include(e=>e.Employee).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return result;
    }

    public async Task<int> CheckIn(AttendanceRecord record, CancellationToken cancellationToken)
    {
        var existing = dbContext.Attendances.FirstOrDefault(x => x.EmployeeId == record.Id && x.Date.Date == DateTime.Today);
        var result = await dbContext.Attendances.AddAsync(record, cancellationToken);
        var res = await dbContext.SaveChangesAsync(cancellationToken);
        return record.Id;
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
        var next = today.AddDays(1);
        var result = await dbContext.Attendances.FirstOrDefaultAsync(x=>
            x.EmployeeId==employeeId && x.Date >= today && x.Date < next,cancellationToken);
        
        if (result == null) return 0;
        result.CheckOut = DateTime.UtcNow;
        result.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return result.Id;
    }
    
    public async Task<AttendanceRecord?> CheckIfAttendanceExistAsync(int employeeId, DateTime date,
        CancellationToken cancellationToken)
    {
        return await dbContext.Attendances
            .Where(a => a.Date.Date >= date.Date && a.EmployeeId == employeeId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken)
    {
        var totalEmployees = await dbContext.Employees.Select(e => e.Id).Distinct().CountAsync(cancellationToken);
        return await dbContext.Attendances.Include(x => x.Employee).ToListAsync(cancellationToken);
        
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Employees.CountAsync((cancellationToken));
    }

    public async Task<int> UpdateBulkAttendanceAsync(AttendanceRecord record, CancellationToken cancellationToken)
    {
         dbContext.Attendances.Update(record);
         var existing = dbContext.Attendances.FirstOrDefault(x => x.EmployeeId == record.Id && x.Date.Date == DateTime.Today);
         await dbContext.Attendances.OrderByDescending(x=>x.Date).ThenByDescending(x=>x.CheckIn).ToListAsync();
         return await dbContext.SaveChangesAsync(cancellationToken);
        
    }

   

    public async Task<int> CountPresentTodayAsync(DateTime today, CancellationToken cancellationToken)
    {
        return await dbContext.Attendances.Where(x=>x.Date.Date == today.Date && x.CheckIn != null).CountAsync(cancellationToken);
    }

    public async Task<List<DepartmentAttendance>> GetDepartmentAttendanceAsync(DateTime today, CancellationToken cancellationToken)
    {
        var result = await dbContext.Departments.Select(d=> new DepartmentAttendance()
            {
                DepartmentName = d.Name,
                EmployeeCount = d.Employees.Count,
                PresentCount = d.Employees
                    .Count(t=> dbContext.Attendances
                        .Any(t=>t.EmployeeId == t.Id && t.Date.Date == today.Date && t.CheckIn != null))
            }
            ).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<List<Activity>> GetRecentActivitiesAsync(int count, CancellationToken cancellationToken)
    {
        return await  dbContext.Activities.OrderByDescending(x=>x.CreatedAt).Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ClockInAsync(int employeeId, DateTime now, CancellationToken cancellationToken)
    {
        var today = now.Date;
        var existing =
            await dbContext.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date >= today,
                cancellationToken);
        if (existing != null)
            return false;
        var attendance = new AttendanceRecord()
        {
            EmployeeId = employeeId,
            Date = today,
            CheckIn = now,
        };
        await dbContext.Attendances.AddAsync(attendance, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;

    }

    public async Task<bool> ClockOutAsync(int employeeId, DateTime now, CancellationToken cancellationToken)
    {
        var today = now.Date;
        var attendance =
            await dbContext.Attendances.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Date.Date == today,
                cancellationToken);
        if (attendance == null)
            return false;
        attendance.CheckOut = now;
        dbContext.Attendances.Update((attendance));
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}