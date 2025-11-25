using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AttendanceService(IAttendanceRepository repository) : IAttendanceService
{
    public async Task<ApiResponse<int>> CheckIn( AttendanceRequest request,  CancellationToken cancellationToken)
    {
        var existingRecord =
            await repository.GetByDateAsync(request.EmployeeId, 
                DateTime.UtcNow.Date, cancellationToken);
        if (existingRecord != null )
        {
            return new ApiResponse<int>()
            {
                Message = "Employee has already checked in today",
                StatusCode = StatusCodes.Status409Conflict
            };
        }
      
        var startTime = DateTime.UtcNow.Date.AddHours(8);  
        var record = new AttendanceRecord
        {
            EmployeeId = request.EmployeeId,
            CheckIn = DateTime.UtcNow,
            Date = DateTime.UtcNow.Date,
            IsLate = DateTime.UtcNow > startTime
        };
        
         var result = await repository.CheckIn(record, cancellationToken);
         if (result < 1)
         { 
             return new ApiResponse<int>()
               {
                   Message = "Failed to check in",
                   Data = record.Id,
                   StatusCode = StatusCodes.Status500InternalServerError
               };
            
         }

        return new ApiResponse<int>
        {
            Data = record.Id,
            Message = "Attendance added successfully",
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ApiResponse<List<AttendanceResponseDto>>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken)
    {
        var res = await repository.GetAllAsync(request, cancellationToken);
        var response = new List<AttendanceResponseDto>();
        foreach (var r in res)
        {
            var result = r.Adapt(new AttendanceResponseDto());
            result.FirstName= r.Employee.FirstName;
            result.Surname = r.Employee.Surname;
            result.Department = r.Employee.Department?.Name;
            response.Add(result);
        }
        return new ApiResponse<List<AttendanceResponseDto>>
        {
            Message = "All attendance records fetched",
            StatusCode = StatusCodes.Status200OK,
            Data = response
        };
    }

    public async Task<ApiResponse<AttendanceResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var res = await repository.GetByIdAsync(id, cancellationToken);
        if (res == null)
        {
            return new ApiResponse<AttendanceResponseDto>()
            {
                Message = "Record Not Found",
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        var result = res.Adapt(new AttendanceResponseDto());

        if (res.Employee != null)
        {
            result.FirstName = res.Employee.FirstName;
            result.Surname = res.Employee.Surname;
        }

        return new ApiResponse<AttendanceResponseDto>()
        {
            Message = "Your Request was successfull",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        }; 

    }

    public async Task<ApiResponse<int>> CheckOutAsync(UpdatedAttendanceRequest request, CancellationToken cancellationToken)
    {
         
       
       var existingRecord = await repository.GetByDateAsync(request.EmployeeId, DateTime.UtcNow.Date, cancellationToken);
       
       if (existingRecord == null || existingRecord.CheckIn == null)
       {
           return new ApiResponse<int>()
           {
               Message = "Employee has not checked in",
               StatusCode = StatusCodes.Status400BadRequest
           };
       }

       if (existingRecord.CheckOut != null)
       {
           return new ApiResponse<int>()
           {
               Message = "Employee has already checked out",
               StatusCode = StatusCodes.Status409Conflict
           };
       }
       
       var result =  await repository.CheckOut(request.EmployeeId, cancellationToken);
       if (result == 0)
       {
           return new ApiResponse<int>()
           {
               Message = "Employee not found",
               StatusCode = StatusCodes.Status404NotFound
           };
       }
       return new ApiResponse<int>
       {
            Message = $"Record updated successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        };
       
    }


    public async Task<ApiResponse<int>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return new ApiResponse<int>
        {
            Message = "Attendance deleted successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ApiResponse<AttendanceSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var lateThreshold = new TimeSpan(8, 10, 0); 

        // Fetch all attendance records for today   
        var todayRecords = await repository.GetAllSummaryAsync(cancellationToken);
        
        var recordsToday = todayRecords.Where(a => a.Date >= today && a.CreatedAt < today.AddDays(1)).ToList();

        var presentToday = recordsToday.Count(r => r.CheckIn.HasValue);
        var lateArrivals = recordsToday.Count(r => r.CheckIn.HasValue && r.CheckIn.Value.TimeOfDay > lateThreshold);
        var totalAttendance = await repository.CountAsync(cancellationToken);
      

        var absent = totalAttendance - presentToday;
        if (absent < 0) absent = 0; // safety check

        var summary = new AttendanceSummaryDto
        {
            TotalEmployees = totalAttendance,
            PresentToday = presentToday,
            LateArrivals = lateArrivals,
            Absent = absent,
         
        };

        return new ApiResponse<AttendanceSummaryDto>
        {
            Data = summary,
            Message = "Attendance summary retrieved successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }
}
