
    using Azure.Core;
    using Microsoft.EntityFrameworkCore;
    using Payroll.Attendance.Application.Dto;
    using Payroll.Attendance.Application.Dto.Employee;
    using Payroll.Attendance.Application.Repositories;
    using Payroll.Attendance.Domain.Enum;
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

            public async Task<List<Employee>> GetAllEmployeesAsync(PaginationRequest request,
                CancellationToken cancellationToken)
            {
                var query =  context.Employees.AsQueryable().AsNoTracking();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query= query.Where(x=>
                        x.EmploymentType.Contains(request.SearchText) ||
                        x.FirstName.Contains(request.SearchText) ||
                        x.OtherName.Contains(request.SearchText) || 
                        x.JobPosition.Contains(request.SearchText) ||
                        x.Surname.Contains( request.SearchText)  ||
                        x.Email.Contains(request.SearchText) ||
                        x.PayFrequency.Contains(request.SearchText) ||
                        x.PhoneNumber.Contains(request.SearchText) ||
                        x.ReportingManager.Contains(request.SearchText) ||
                        x.Salary.Contains(request.SearchText) ||
                        x.Address.Contains(request.SearchText)
                        
                        
                        );
                }

                if (request.StartDate.HasValue && request.EndDate.HasValue)
                {
                    query = query.Where(x =>x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate);
                }
                
                int skip = (request.PageNumber - 1) * request.PageSize;
                
                var data = await query
                    .Skip(skip)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
                return data;
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

            public async  Task<List<EmployeeIdAndNameDto>> GetEmployeeIdAndName(string? searchText, CancellationToken cancellationToken)
            {
                var query = context.Employees.AsQueryable().AsNoTracking();

                if (!string.IsNullOrEmpty(searchText))
                    return await query
                        .Select(x => new EmployeeIdAndNameDto
                            {
                                Id = x.Id,
                                FullName = x.Title+ " " + x.FirstName + " " + x.Surname.Trim(),
                            }
                        )
                        .OrderBy(x => x.FullName)
                        .ToListAsync(cancellationToken);
                {
                    var searchTerm = searchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.FirstName.Contains(searchText) ||
                        x.Surname.Contains(searchText) ||
                        x.OtherName != null && x.OtherName.Contains(searchText));
                }
                return await query
                    .Select(x => new EmployeeIdAndNameDto
                        {
                            Id = x.Id,
                            FullName = x.Title + x.FirstName + " " + x.Surname.Trim(),
                        }
                    )
                    .OrderBy(x => x.FullName)
                    .ToListAsync(cancellationToken);
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

           

            public async Task<Employee> GetByIdAsync(string employeeId, CancellationToken ct)
            {
                return await context.Employees.FirstOrDefaultAsync(x => x.Id.ToString() == employeeId,ct);
            }

            public async Task<EmployeeSummary?> GetEmployeeSummaryAsync(CancellationToken cancellationToken)
            {
                var employee = await context.Employees.ToListAsync(cancellationToken);
                return new EmployeeSummary
                {
                    TotalEmployee = employee.Count,
                    NSSPersonnel = employee.Count(e => e.EmploymentType == "Nss"),
                    FullTime = employee.Count(e => e.EmploymentType == "FullTime"),
                    PartTime = employee.Count(e => e.EmploymentType == "PartTime"),
                    Others = employee.Count(e=>e.EmploymentType == "Others"),
                    interns = employee.Count(e => e.EmploymentType == "Intern"),
                    ActiveEmployee = employee.Count(x => x.IsActive),
                    InActiveEmployee = employee.Count(e => e.IsActive==false)
                };
            }

           

            public async Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken ct)
            {
             return await context.Employees.Where(e => e.DepartmentId == departmentId).OrderBy(e=>e.FirstName).ToListAsync(ct); 
            }

            public async Task<IEnumerable<Employee>> GetByDepartmentAsync(CancellationToken ct)
            {
               return await context.Employees.OrderBy(e => e.FirstName).ToListAsync(ct);
            }

            


        }
    }     