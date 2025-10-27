
    using Microsoft.EntityFrameworkCore;
    using Payroll.Attendance.Application.Repositories;
    using Payroll.Attendance.Domain.Models;
    using Payroll.Attendance.Infrastructure.Data;


    namespace Payroll.Attendance.Infrastructure.Repositories
    {
        public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
        {
            public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
            {
                await context.Employees.AddAsync(employee, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
            {
                return await context.Employees.ToListAsync(cancellationToken);
            }

            public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
            {
                return await context.Employees.FirstOrDefaultAsync(e => Equals(e.Id, id), cancellationToken);
            }

            public async Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
            {
                context.Employees.Update(employee);
                await context.SaveChangesAsync(cancellationToken);
            }

            public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
            {
                var employee = await context.Employees.FindAsync(new object[] { id }, cancellationToken);
                if (employee != null)
                {
                    context.Employees.Remove(employee);
                    await context.SaveChangesAsync(cancellationToken);
                }
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