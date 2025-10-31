using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AttendanceCalculator : IAttendanceCalculator
{
    public void CalculateAttendanceDe9tails(AttendanceRecord attendance, SystemSettings settings)
    {
        throw new NotImplementedException();
    }

    public void CalculateAttendanceDetails(AttendanceRecord attendance, SystemSettings settings)
    {
        throw new NotImplementedException();
    } 

    public int GetWorkingDaysInMonth(string month, SystemSettings settings)
    {
        throw new NotImplementedException();
    }
}