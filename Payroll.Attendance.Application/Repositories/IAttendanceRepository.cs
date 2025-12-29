using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> CheckIn(AttendanceRecord attendance, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetAllAsync(GetAttendanceRequest request,
            CancellationToken cancellationToken);
        Task<AttendanceRecord?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task  DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int> CheckOut(int employeeId, CancellationToken cancellationToken);
        
        Task<AttendanceRecord?> CheckIfAttendanceExistAsync(int employeeId, DateTime date,
            CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetAllSummaryAsync(CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<int> UpdateBulkAttendanceAsync(AttendanceRecord record, CancellationToken cancellationToken);
        Task<int> UpdateAttendanceAsync(AttendanceRecord record, CancellationToken cancellationToken);

        Task<int> CountPresentTodayAsync(DateTime today, CancellationToken cancellationToken);
        Task<List<DepartmentAttendance>> GetDepartmentAttendanceAsync(DateTime today,
            CancellationToken cancellationToken);
     
        Task<bool> ClockInAsync(int employeeId, DateTime now, CancellationToken cancellationToken);
        Task<bool> ClockOutAsync(int employeeId, DateTime now, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetTodayAttendanceAsync( CancellationToken cancellationToken);
        Task<List<AttendanceRecord>>GetTodayAttendanceWithoutTokenAsync(CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetOnlyTodayAttendanceSummary(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetOnlyWeekAttendanceSummary(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetOnlyMonthAttendanceSummary(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetAttendanceByEmployeeIdAsync (DateTime startDate, DateTime endDate,int employeeId, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> GetUncheckAttendanceAsync(DateTime date, CancellationToken cancellationToken);
        Task<List<AttendanceRecord>> UpdateAttendanceCheckoutAsync(List<AttendanceRecord> attendance,
            CancellationToken cancellationToken);

        Task<decimal> GetOverallAttendanceRateAsync(int month, int year, CancellationToken cancellationToken);

    }
}