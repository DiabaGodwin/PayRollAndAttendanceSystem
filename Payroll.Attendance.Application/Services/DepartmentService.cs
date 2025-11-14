using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.Department;
using Payroll.Attendance.Application.Dto.Employee;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class DepartmentService(IDepartmentRepository departmentRepository,ILogger<DepartmentService> logger) : IDepartmentService
{
    public async Task<ApiResponse<int>> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await departmentRepository.GetDepartmentByNameAsync(createDepartmentDto.Name.Trim(), cancellationToken);
            if (response != null)
            {
                return new ApiResponse<int>
                {
                    Message = "Department already exists.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            
            var department = createDepartmentDto.Adapt<Department>();
            department.CreatedAt = DateTime.Now;
            
            var departmentId = await departmentRepository.CreateDepartment(department, cancellationToken);
            if (departmentId > 0)
            {
                return new ApiResponse<int>()
                {
                    Message = "Department created successfully",
                    StatusCode = StatusCodes.Status201Created
                };
            }

            return new ApiResponse<int>()
            {
                Message = "Failed to create department.",
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        catch (System.Exception e)
        {
           logger.LogError( "Error  creating department with name {Name}.", createDepartmentDto.Name);
           return new ApiResponse<int>()
           {
               Message = "Failed to create department.",
               StatusCode = StatusCodes.Status500InternalServerError
           };
        }
        
    }

    public async Task<ApiResponse<List<DepartmentResponseDto>>> GetAllDepartmentsAsync(PaginationRequest request, CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllDepartmentsAsync(request, cancellationToken);
        var data = departments.Adapt(new List<DepartmentResponseDto>());

        
        return new ApiResponse<List<DepartmentResponseDto>>
        {
            Message = "All departments successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = data
        };

    }

    //Todo: Complete this method
    public async Task<ApiResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await departmentRepository.GetDepartmentByIdAsync(id, cancellationToken);
        var result = response.Adapt<DepartmentResponseDto>();
        
        if (response == null)
        {
            return new ApiResponse<DepartmentResponseDto>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Request was successfully retrieved",
                Data = result
            };
            
        }

        return new ApiResponse<DepartmentResponseDto>()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "Request failed",
            Data = null
        };
        ; 
    }
    
    
    


    public async Task<ApiResponse<bool>> UpdateDepartmentAsync(UpdateDepartmentRequest updateDepartmentRequest,
        CancellationToken cancellationToken)
    {
         try
         {
             
                var existingDepartment = await departmentRepository.GetDepartmentByIdAsync(updateDepartmentRequest.Id, cancellationToken);
                if (existingDepartment == null)
                {
                    return new ApiResponse<bool>
                    {
                      
                        Message = $"Department with ID {updateDepartmentRequest.Id} not found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

               
                var departmentWithSameName = await departmentRepository.GetDepartmentByNameAsync(updateDepartmentRequest.Name.Trim(), cancellationToken);
                if (departmentWithSameName != null && departmentWithSameName.Id != updateDepartmentRequest.Id)
                {
                    return new ApiResponse<bool>
                    {
                       
                        Message = $"Department with name '{updateDepartmentRequest.Name}' already exists",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

               
                updateDepartmentRequest.Adapt(existingDepartment);
                existingDepartment.UpdatedAt = DateTime.UtcNow;

                var result = await departmentRepository.UpdateDepartmentAsync(existingDepartment, cancellationToken);
                
                return new ApiResponse<bool>
                {
                  
                    Message = result ? "Department updated successfully" : "Failed to update department",
                    Data = result,
                    StatusCode = result ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError
                };
         }
         catch (System.Exception ex)
         {
                logger.LogError(ex, "Error updating department {Id}", updateDepartmentRequest.Id);
                return new ApiResponse<bool>
                {
                    
                    Message = "An error occurred while updating department",
                    Data = false,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
         }
    }

    public async Task<ApiResponse<bool>> DeleteDepartmentAsync(int id, CancellationToken cancellationToken)
    {
        try {
                // Check if department exists
                var departmentExists = await departmentRepository.DepartmentExistsAsync(id, cancellationToken);
                if (!departmentExists)
                {
                    return new ApiResponse<bool>
                    {
                      
                        Message = $"Department with ID {id} not found",
                        Data = false,
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                // Check if department has employees
                var hasEmployees = await departmentRepository.HasEmployeesAsync(id, cancellationToken);
                if (hasEmployees)
                {
                    return new ApiResponse<bool>
                    {
                      
                        Message = "Cannot delete department that has employees. Please reassign or remove employees first.",
                        Data = false,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                var result = await departmentRepository.DeleteDepartmentAsync(id, cancellationToken);
                
                return new ApiResponse<bool>
                {
                    
                    Message = result ? "Department deleted successfully" : "Failed to delete department",
                    Data = result,
                    StatusCode = result ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError
                };
        }
        catch (System.Exception ex)
        { 
                logger.LogError(ex, "Error deleting department {Id}", id);
                return new ApiResponse<bool>
                {
                    
                    Message = "An error occurred while deleting department",
                   
                    StatusCode = StatusCodes.Status500InternalServerError
                };
        }
    }

    public async Task<ApiResponse<List<GetOnlyDepartmentDto>>> GetAllOnlyDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        var res = await departmentRepository.GetAllOnlyDepartmentsAsync(cancellationToken);
        var result = res.Adapt(new List<GetOnlyDepartmentDto>());
        return new ApiResponse<List<GetOnlyDepartmentDto>>()
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Retrieved department successfully",
            Data = result
        };
    }
}