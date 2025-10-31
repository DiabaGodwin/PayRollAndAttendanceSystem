using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AttendanceCalculator : IAttendanceCalculator
{
    public void CalculateAttendanceDetails(Domain.Models.AttendanceRecord attendanceRecord, SystemSettings settings)
    {
        throw new NotImplementedException();
    }

    public int GetWorkingDaysInMonth(string month, SystemSettings settings)
    {
        throw new NotImplementedException();
    }
}