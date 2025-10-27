using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class PayrollService
    {
        private readonly IPayrollRecordRepository _payrollRecordRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public PayrollService(
            IPayrollRecordRepository payrollRecordRepository,
            IAttendanceRepository attendanceRecordRepository)
        {
            _payrollRecordRepository = payrollRecordRepository;
            _attendanceRepository = attendanceRecordRepository;
        }

       
        
    }
}