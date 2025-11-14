using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger) : IDepartmentRepository
{
    public async Task<int> CreateDepartment(Department department, CancellationToken cancellationToken)
    {
        var response  = await context.Departments.AddAsync(department, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Department>> GetAllDepartmentsAsync(PaginationRequest request, CancellationToken cancellationToken = default)
    {
        var query =  context.Departments.AsQueryable();
        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query= query.Where(x=>
                x.Name.Contains(request.SearchText)
                        
                        
            );
        }

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

    public async Task<List<Department>> GetAllOnlyDepartmentsAsync(CancellationToken cancellationToken = default)
        => await context.Departments.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Department?> GetDepartmentByIdAsync(int id,
        CancellationToken cancellationToken = default)
    {
        return await context.Departments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Department?> GetDepartmentByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Departments.AsQueryable().FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<bool> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken)
    {
        try
        {
            var existingDepartment = context.Departments.FirstOrDefault(x => x.Id == department.Id);
            if (existingDepartment == null)
                return false;
            existingDepartment.Name = department.Name;
            existingDepartment.Description = department.Description;
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            context.Departments.Update(existingDepartment);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
        catch (Exception e)
        {
            logger.LogError("Error occured updating department with ID {Id}", e);
            throw;

        }
    }

    public Task<bool> DeleteDepartmentAsync(Expression<Func<Department, bool>> id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DepartmentExistsAsync(Expression<Func<Department, bool>> id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var department = await context.Departments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (department == null)
                return false;
            context.Departments.Remove(department);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
        catch (Exception ex)
        {
           logger.LogError("Error Occured deleting department with ID {Id}", ex);
           throw;
        }
       
    }

    public async Task<bool> DepartmentExistsAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await context.Departments.AsNoTracking().AnyAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError("Error occured checking department with ID {Id}", e);
            throw;
        }
    }

    public Task<bool> DepartmentExistsAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            return context.Departments.AsNoTracking().AnyAsync(x => x.Name.ToLower() == name.Trim().ToLower(), cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError("Error occured checking department with Name {Name}", e);
            throw;
        }
    }

    public async Task<bool> HasEmployeesAsync(int departmentId, CancellationToken cancellationToken)
    {
        return await context.Employees.AsNoTracking().AnyAsync(x => x.DepartmentId == departmentId, cancellationToken);
        
    }
}