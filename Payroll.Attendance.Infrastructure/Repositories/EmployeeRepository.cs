using Microsoft.EntityFrameworkCore;
    using Payroll.Attendance.Application.Dto;
    using Payroll.Attendance.Application.Dto.Employee;
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
                        x.JobPosition!.Contains(request.SearchText) ||
                        x.Surname.Contains( request.SearchText)  ||
                        x.Email.Contains(request.SearchText) ||
                        x.PayFrequency.Contains(request.SearchText) ||
                        x.PhoneNumber.Contains(request.SearchText) ||
                        x.ReportingManager!.Contains(request.SearchText) ||
                        x.Salary.Contains(request.SearchText) ||
                        x.Address!.Contains(request.SearchText)
                        
                        
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

            public async Task<bool> UpdateEmployeeAsync(int id,UpdateEmployeeRequest request, CancellationToken cancellationToken)
            {
                await context.Employees.Where(x => x.Id == id).ExecuteUpdateAsync(y => y
                        .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                        .SetProperty(x => x.JobPosition, request.JobPosition)
                        .SetProperty(x => x.Surname, request.Surname)
                        .SetProperty(x => x.FirstName, request.FirstName)
                        .SetProperty(x=> x.PayFrequency, request.PayFrequency)
                        .SetProperty(x => x.PhoneNumber, request.PhoneNumber)
                        .SetProperty(x => x.Email, request.Email)   
                        .SetProperty(x=> x.Salary, request.Salary)
                        .SetProperty(x => x.Address, request.Address)
                        .SetProperty(x=>x.Title, request.Title)
                        .SetProperty(x=>x.OtherName, request.OtherName)
                        .SetProperty(x=>x.DateOfBirth, request.DateOfBirth)
                        .SetProperty(x=> x.DepartmentId, request.DepartmentId)
                        .SetProperty(x=> x.EmploymentType, request.EmploymentType)
                    
                    , cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken);
                return result > 0;
            }

            public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
            {
                await context.Employees.Where(x => x.Id == id)
                    .ExecuteUpdateAsync(x => x
                    .SetProperty(y=> y.IsActive,false),cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken);
                return result > 0;
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
                    var searchTerm = searchText!.Trim().ToLower();
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

          
 
            public async Task<Employee?> GetByIdAsync(int employeeId)
            {
               var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
               return employee;
            }

            public async Task<Employee?> GetByEmailAsync(string email, CancellationToken ct)
            { 
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Email == email, ct);
                return employee;
            }

           

            public async Task<Employee?> GetByIdAsync(string employeeId, CancellationToken ct)
            {
                return await context.Employees.FirstOrDefaultAsync(x => x.Id.ToString() == employeeId,ct);
            }

            public async Task<EmployeeSummary?> GetEmployeeSummaryAsync(CancellationToken cancellationToken)
            {
                var employee = await context.Employees.ToListAsync(cancellationToken);
                return new EmployeeSummary
                {
                    TotalEmployee = employee.Count,
                    NssPersonnel = employee.Count(e => e.EmploymentType == "Nss"),
                    FullTime = employee.Count(e => e.EmploymentType == "FullTime"),
                    PartTime = employee.Count(e => e.EmploymentType == "PartTime"),
                    Others = employee.Count(e=>e.EmploymentType == "Others"),
                    Interns = employee.Count(e => e.EmploymentType == "Intern"),
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