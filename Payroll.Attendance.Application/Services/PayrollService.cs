using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class PayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public PayrollService(
            IPayrollRepository payrollRepository,
            IAttendanceRepository attendanceRecordRepository)
        {
            _payrollRepository = payrollRepository;
            _attendanceRepository = attendanceRecordRepository;
        }

       
        
    }
}