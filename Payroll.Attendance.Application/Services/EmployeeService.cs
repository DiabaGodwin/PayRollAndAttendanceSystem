using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

   
namespace Payroll.Attendance.Application.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository,ILogger<EmployeeService> logger) :IEmployeeService
    {
        public async Task<ApiResponse<int>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token)
        {
            logger.LogInformation("Adding new employee");

            if (!Enum.TryParse<EmploymentType>(addEmployeeDto.EmploymentType, true, out  var employmentType ))
            {
                return new ApiResponse<int>
                {
                    Message = $"Invalid employment type {addEmployeeDto.EmploymentType}",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            if (!Enum.TryParse<PayFrequency>(addEmployeeDto.PayFrequency, true, out var payFrequency))

            {
                return new ApiResponse<int>
                {
                    Message =  $"Invalid pay frequency {addEmployeeDto.PayFrequency}. Allowed values: {string.Join(", ", Enum.GetNames(typeof(PayFrequency)))}",
                       
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }

         
                
                
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
                EmploymentType = addEmployeeDto.EmploymentType,
                DepartmentId = addEmployeeDto.DepartmentId,
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

        public async  Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            var res = await employeeRepository.GetAllEmployeesAsync(request,cancellationToken);
            var result = res.Adapt(new List<EmployeeResponseDto>());
            return new ApiResponse<List<EmployeeResponseDto>>
            {
                Message = "Your request was succefully retrieved",
                StatusCode = StatusCodes.Status200OK,
                Data =  result
            };
        }
        /*
            return new ApiResponse<EmployeeResponseDto>()
            {
                Message = "An error occur when requesting",
                StatusCode = StatusCodes.Status400BadRequest,
            };
            */
        
        public async Task<Employee> UpdateEmployeeAsync(Employee updateEmployeeDto,
            CancellationToken cancellationToken)
        {
            return await employeeRepository.UpdateEmployeeAsync(updateEmployeeDto, cancellationToken);
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
