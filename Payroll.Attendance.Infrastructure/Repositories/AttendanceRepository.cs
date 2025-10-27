using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;
using Payroll.Attendance.Infrastructure.Data;

namespace Payroll.Attendance.Infrastructure.Repositories;

public class AttendanceRepository(ApplicationDbContext context) : IAttendanceRepository
{
    public Task RecordAttendanceAsync(Domain.Models.Attendance request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAttendanceAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetAttendanceByEmployeeIdAsync(int empId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}