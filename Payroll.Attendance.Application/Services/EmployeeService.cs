using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task AddEmployeeAsync(Employee employee, CancellationToken token)
        {
            await _employeeRepository.AddEmployeeAsync(employee, token);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken token)
        {
            return await _employeeRepository.GetAllEmployeesAsync(token);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken token)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id, token);
        }
    }
}