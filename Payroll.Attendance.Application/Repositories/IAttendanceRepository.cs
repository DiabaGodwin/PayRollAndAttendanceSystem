using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> CheckIn(AttendanceRecord attendance, CancellationToken cancellationToken);
        Task<IEnumerable<AttendanceRecord>> GetAllAsync(CancellationToken cancellationToken);
        Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int> CheckOut(int employeeId, CancellationToken cancellationToken);
        
        Task<IEnumerable<AttendanceRecord>> GetByDateAsync(DateTime date, CancellationToken cancellationToken);
        
    }
}