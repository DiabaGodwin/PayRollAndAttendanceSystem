
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories;
public interface IAttendanceRepository
{
    Task RecordAttendanceAsync(Domain.Models.Attendance request, CancellationToken ct);
    Task DeleteAttendanceAsync(int id,CancellationToken ct);
    Task<Employee> GetAttendanceByEmployeeIdAsync(int empId, CancellationToken ct);
}

