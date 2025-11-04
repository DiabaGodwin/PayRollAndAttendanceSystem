using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public interface IAttendanceCalculator
{
    void CalculateAttendanceDetails(Domain.Models.AttendanceRecord attendanceRecord, SystemSettings settings);
    int GetWorkingDaysInMonth(string month, SystemSettings settings);
}