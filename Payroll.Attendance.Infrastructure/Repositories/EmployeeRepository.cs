
    using Microsoft.EntityFrameworkCore;
    using Payroll.Attendance.Application.Repositories;
    using Payroll.Attendance.Domain.Models;
    using Payroll.Attendance.Infrastructure.Data;


    namespace Payroll.Attendance.Infrastructure.Repositories
    {
       
        
        public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
        {
            public async Task<int> AddEmployeeAsync(Employee employee, CancellationToken token)
            {
             await context.Employees.AddAsync(employee, token);
             var result = await context.SaveChangesAsync(token);
             return result;
            }

            public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken)
            { 
                return await context.Employees.OrderBy(e => e.FirstName).ToListAsync(cancellationToken);
                   
            }

            public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                return employee;
            }

            public async Task<Employee> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
            { 
                context.Entry(employee).State = EntityState.Modified;
                employee.UpdatedAt = DateTime.UtcNow;
                context.SaveChangesAsync(cancellationToken);
               return employee;
               
            }

            public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (employee == null)
                
                    return false;
                context.Employees.Remove(employee);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }

            public async Task<Employee> GetByIdAsync(int employeeId)
            {
               var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
               return employee;
            }

            public async Task<Employee> GetByEmailAsync(string email, CancellationToken ct)
            { 
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Email == email, ct);
                return employee;
            }

            public async Task<Employee?> GetByIdAsync(string employeeId, CancellationToken ct)
            {
                return await context.Employees.FirstOrDefaultAsync(x => x.Id.ToString() == employeeId);
            }

            public async Task<EmployeeSummary?> GetByEmployeeSummaryAsync(string username, CancellationToken ct)
            {
                var employee = await context.Employees.ToListAsync();
                return new EmployeeSummary
                {
                    TotalEmployee = employee.Count,
                    NSSPersonnel = employee.Count(e => e.Category == "NSS"),
                    interns = employee.Count(e => e.Category == "Interns"),
                    ActiveEmployee = employee.Count(e => e.EmploymentStatus == "Active"),
                    InActiveEmployee = employee.Count(e => e.EmploymentStatus == "InActive")
                };
            }

            public async Task<List<Employee>> GetEmployeesByDepartmentAsync(string department, CancellationToken ct)
            {
             return await context.Employees.Where(e => e.Department == department).OrderBy(e=>e.FirstName).ToListAsync(ct); 
            }

            public async Task<IEnumerable<Employee>> GetByDepartmentAsync(CancellationToken ct)
            {
               return await context.Employees.OrderBy(e => e.FirstName).ToListAsync(ct);
            }

            


        }
    }     