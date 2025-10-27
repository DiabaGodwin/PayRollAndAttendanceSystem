using Payroll.Attendance.Application.Dto;

namespace Payroll.Attendance.Application.Services;

public interface IAttendanceCalculator
{
    void CalculateAttendanceDetails(Domain.Models.Attendance attendance, SystemSettings settings);
    int GetWorkingDaysInMonth(string month, SystemSettings settings);
}