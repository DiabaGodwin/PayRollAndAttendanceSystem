using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository,ILogger<EmployeeService> logger) :IEmployeeService
    {
        public async Task<ApiResponse<int>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token)
        {
            logger.LogInformation("Adding new employee");
            
            var employee = new Employee
            {
                Title = addEmployeeDto.Title,
                FirstName = addEmployeeDto.FirstName,
                Surname = addEmployeeDto.Surname,
                OtherName = addEmployeeDto.OtherName,
                Email = addEmployeeDto.Email,
                PhoneNumber = addEmployeeDto.PhoneNumber,
                DateOfBirth = addEmployeeDto.DateOfBirth,
                Address = addEmployeeDto.Address,
                Salary = addEmployeeDto.Salary,
                Department = addEmployeeDto.Department,
                EmploymentType = addEmployeeDto.EmploymentType,
                HireDate = addEmployeeDto.HireDate,
                JobPosition = addEmployeeDto.JobPosition,
                PayFrequency = addEmployeeDto.PayFrequency,
            };
            var addedEmployee = await employeeRepository.AddEmployeeAsync(employee, token);
            if (addedEmployee < 1)
            {
                return new ApiResponse<int>
                {
                    Message = "Failed to create employee",
                    Data = 0,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            
            return new ApiResponse<int>()
            {
                StatusCode = StatusCodes.Status200OK,
            
            };
        }

        public Task<ApiResponse<EmployeeResponseDto>> GetAllEmployeesAsync(EmployeeResponseDto cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee updateEmployeeDto,
            CancellationToken cancellationToken)
        {
            return await employeeRepository.UpdateEmployeeAsync(updateEmployeeDto, cancellationToken);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken token)
        {
            return await employeeRepository.GetAllEmployeesAsync(token);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken token)
        {
            return await employeeRepository.GetEmployeeByIdAsync(id, token);
        }

        public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            return await employeeRepository.DeleteEmployeeAsync(id,  cancellationToken );
        }

       
        
    }
}