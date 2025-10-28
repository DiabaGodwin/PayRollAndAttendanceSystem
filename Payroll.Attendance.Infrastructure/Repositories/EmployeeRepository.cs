
    using Microsoft.EntityFrameworkCore;
    using Payroll.Attendance.Application.Repositories;
    using Payroll.Attendance.Domain.Models;
    using Payroll.Attendance.Infrastructure.Data;


    namespace Payroll.Attendance.Infrastructure.Repositories
    {
        public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
        {
            public Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<object> GetByIdAsync(int employeeId)
            {
                throw new NotImplementedException();
            }

            public Task<object> GetByIdAsync(string employeeId, CancellationToken ct)
            {
                throw new NotImplementedException();
            }
        }
    }     