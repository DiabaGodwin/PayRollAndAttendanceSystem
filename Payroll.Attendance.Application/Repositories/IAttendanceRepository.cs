using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> CheckIn(AttendanceRecord attendance, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken);
        Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task  DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int> CheckOut(int employeeId, CancellationToken cancellationToken);
        
        Task<AttendanceRecord?> GetByDateAsync(int employeeId, DateTime date,
            CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(AttendanceRecord record, CancellationToken cancellationToken);
     
        
    }
}