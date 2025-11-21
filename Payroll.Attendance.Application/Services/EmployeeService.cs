using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;


namespace Payroll.Attendance.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
    : IEmployeeService
{
    private (bool Success, string Message) ValidateDateOfBirth(DateTime? dob)
    {
        if (dob == null)
            return (false, "Date of birth is required");

        if (dob > DateTime.Today)
            return (false, "Date of birth cannot be in the future");

        // Calculate age
        int age = DateTime.Today.Year - dob.Value.Year;
        if (dob.Value > DateTime.Today.AddYears(-age)) 
            age--;

        // Accept only if age ≥ 18
        if (age < 18)
            return (false, "Employee must be at least 18 years old");

        return (true, "Valid");
    }

    
    public async Task<ApiResponse<int>> AddEmployeeAsync(AddEmployeeDto addEmployeeDto, CancellationToken token)
    {
        logger.LogInformation("Adding new employee");

        if (!Enum.TryParse<EmploymentType>(addEmployeeDto.EmploymentType, true, out _))
        {
            return new ApiResponse<int>
            {
                Message = $"Invalid employment type {addEmployeeDto.EmploymentType}.Allowed values: {string.Join(", ", Enum.GetNames(typeof(EmploymentType)))}",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        if (!Enum.TryParse<PayFrequency>(addEmployeeDto.PayFrequency, true, out _))

        {
            return new ApiResponse<int>
            {
                Message =
                    $"Invalid pay frequency {addEmployeeDto.PayFrequency}. Allowed values: {string.Join(", ", Enum.GetNames(typeof(PayFrequency)))}",

                StatusCode = StatusCodes.Status400BadRequest,
            };
        }
        
        var emailExists = await employeeRepository.EmailExistsAsync(addEmployeeDto.Email, token);
        if (emailExists)
        {
            return new ApiResponse<int>()
            {
                Message = $"Email '{addEmployeeDto.Email}' already exist",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        var dobCheck = ValidateDateOfBirth(addEmployeeDto.DateOfBirth);
        if (!dobCheck.Success)
        {
            return new ApiResponse<int>()
            {
                Message = dobCheck.Message,
                StatusCode = StatusCodes.Status400BadRequest
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
            Message = "Successfully added employee",

        };
    }

    public async Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync(PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var res = await employeeRepository.GetAllEmployeesAsync(request, cancellationToken);
        var result = res.Adapt(new List<EmployeeResponseDto>());
        return new ApiResponse<List<EmployeeResponseDto>>
        {
            Message = "Your request was succefully retrieved",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        };
        
    }
    
        
        

    public async Task<ApiResponse<bool>> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request,CancellationToken cancellationToken)
    {
        var  result = await employeeRepository.UpdateEmployeeAsync(id,request, cancellationToken);
        if (result)
        {
            return new ApiResponse<bool>()
            {
                Message = "Your request was successfully updated",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            };
        }
        return new ApiResponse<bool>()
        {
            Message = "Your request failed",
            StatusCode = StatusCodes.Status400BadRequest,
            Data = false
        };

    }



    public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        var res = await employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);
        var response = res.Adapt(new EmployeeResponseDto());

        return new ApiResponse<EmployeeResponseDto>()
        {
            Message = "Your request was successful retrieved",
            StatusCode = StatusCodes.Status200OK,
            Data = response
    
        };


    }



    public async Task<ApiResponse<List<EmployeeIdAndNameDto>>> GetEmployeeIdAndName(string? searchText,
        CancellationToken cancellationToken)
    {
        var res = await employeeRepository.GetEmployeeIdAndName(searchText, cancellationToken);
        var result  = res.Adapt(new List<EmployeeIdAndNameDto>());
        return new ApiResponse<List<EmployeeIdAndNameDto>>()
        {
            Message = "Your request was successful retrieved",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        };
            

    }

    public async Task<ApiResponse<List<EmployeeResponseDto>>> DeleteEmployeeAsync(int id,
        CancellationToken cancellationToken)
    {
        var res = await employeeRepository.DeleteEmployeeAsync(id, cancellationToken);
        var result = res.Adapt(new List<EmployeeResponseDto>());
         
        return new ApiResponse<List<EmployeeResponseDto>>
        {
            Message = "Your request was successful deleted",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        };
    }




    public async Task<ApiResponse<EmployeeSummaryDto>> GetEmployeeSummaryAsync(CancellationToken cancellationToken)
    {
        try
        {
            var employees = await employeeRepository.GetAllEmployeesAsync(
                new PaginationRequest { PageNumber = 1, PageSize = 10000 }, 
                cancellationToken);

            var summary = new EmployeeSummaryDto
            {
                TotalEmployee = employees.Count,
                NSSPersonnel = employees.Count(e => e.EmploymentType == "NssPersonnel"),
                FullTime = employees.Count(e => e.EmploymentType == "FullTime"),
                PartTime = employees.Count(e => e.EmploymentType == "PartTime"),
                Interns = employees.Count(e => e.EmploymentType == "Intern"),
                ActiveEmployee = employees.Count(x => x.IsActive),
                InActiveEmployee = employees.Count(e => e.IsActive==false)
                    
            };

            return new ApiResponse<EmployeeSummaryDto>
            {
                   
                Message = "Employee summary retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = summary,
            };
        }
        catch (System.Exception)
        {
                
            return new ApiResponse<EmployeeSummaryDto>
            {
                   
                Message = "An error occurred while retrieving employee summary",
                StatusCode = StatusCodes.Status500InternalServerError,
                Data = null,
            };
        }
    }
}