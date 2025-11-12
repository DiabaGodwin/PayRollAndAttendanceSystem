using System.Linq.Expressions;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;

public interface IDepartmentRepository
{
    Task<int> CreateDepartment(Department department, CancellationToken cancellationToken);
    Task<List<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<List<Department>> GetAllOnlyDepartmentsAsync( CancellationToken cancellationToken = default);
    Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Department> GetDepartmentByNameAsync(string name,  CancellationToken cancellationToken = default);
    Task<bool> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken);
    Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken);
    Task<bool> DepartmentExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> DepartmentExistsAsync(string name,  CancellationToken cancellationToken);
    Task<bool> HasEmployeesAsync(int departmentId, CancellationToken cancellationToken);
     
}