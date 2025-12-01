using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AttendanceService(IAttendanceRepository repository,  ILogger<AttendanceService> logger, IEmployeeRepository employeeRepository) : IAttendanceService
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
       var employees = await employeeRepository.GetAllEmployeesAsync(new PaginationRequest{PageNumber = 1, PageSize = 1000}, cancellationToken);
       var totalEmployees =  employees.Count;
        
      
        
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var lateThreshhold = new TimeSpan(8, 0, 0);
        
        
        var allRecords = await repository.GetAllSummaryAsync(cancellationToken);
       
        //Daily Summary
        var recordsToday = allRecords.Where(x=>x.Date.Date == today ).ToList();
        var presentToday = recordsToday.Where(e => e.CheckIn.HasValue).Select(x=>x.EmployeeId).Distinct().Count();
        var lateArrivals = recordsToday.Count(e=>e.CheckIn.HasValue && e.CheckIn.Value.TimeOfDay > lateThreshhold);
        var OnLeaveToday = recordsToday.Where(x=>x.IsOnLeave == true).Select(x=>x.EmployeeId).Distinct().Count();
        var absentToday  = totalEmployees - (OnLeaveToday + presentToday);
        if(absentToday < 0) absentToday = 0;
        double attendancePercentage = totalEmployees == 0 ? 0
            : Math.Round(((double)presentToday / totalEmployees) * 100, 1);
        
        //MONTH SUMMARY
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var nextMonth = firstDayOfMonth.AddMonths(1);
        var monthRecords = allRecords.Where(x=>x.Date >= firstDayOfMonth && x.Date <= nextMonth).ToList();
        var workingDaysMonth = monthRecords.Select(x => x.Date.Date).Distinct().Count();
        var overallPresentMonth = monthRecords.Count(x => x.CheckIn.HasValue);
        var overallLeaveMonth = monthRecords.Count(x => x.IsOnLeave == true);
        var overallExpectedMonth = totalEmployees * workingDaysMonth;
        var overallAbsentMonth = overallExpectedMonth - (overallPresentMonth + overallLeaveMonth);
        if(overallAbsentMonth < 0) overallAbsentMonth = 0;
        double averageAttendancePercentageMonth = overallExpectedMonth == 0
            ? 0
            : Math.Round(((double)overallPresentMonth / overallExpectedMonth) * 100, 1);
        
        //ALL-TIME SUMMARY
        var workingDaysAll = allRecords.Select(e => e.Date.Date).Distinct().Count();
            var totalPresent = allRecords.Count(e=>e.CheckIn.HasValue);
            var totalLeave = allRecords.Count(x=>x.IsOnLeave == true);
            var totalExpectedAll = totalEmployees * workingDaysAll;

            var totalAbsent = totalExpectedAll - (totalPresent + totalLeave);
            if(totalAbsent < 0) totalAbsent = 0;
            double lifetimeAttendancePercentage =
                totalExpectedAll == 0 ? 0 : Math.Round(((double)totalPresent / totalExpectedAll) * 100, 2);

            var summary = new AttendanceSummaryDto()
            {
                TotalEmployees = totalEmployees,
                Today = new TodaySummaryDto
                {
                    PresentToday = presentToday,
                    AbsentToday = absentToday,
                    LateArrivals = lateArrivals,
                    OnLeave = OnLeaveToday,
                    AttendancePercentage = attendancePercentage
                },
                Month = new MonthSummaryDto()
                {
                    WorkingDays = workingDaysMonth,
                    OverallPresent =  overallPresentMonth,
                    OverallAbsent = overallAbsentMonth,
                    OverallLeave = overallLeaveMonth,
                    AverageAttendancePercentage = averageAttendancePercentageMonth
                    
                },
                AllTime = new AllTimeSummaryDto()
                {
                   AllWorkingDays = workingDaysAll,
                   TotalPresent = totalPresent,
                   TotalAbsent =  totalAbsent,
                   TotalLeave = totalLeave,
                   LifetimeAttendancePercentage = lifetimeAttendancePercentage
                }
                
            };
        

        return new ApiResponse<AttendanceSummaryDto>
        {
            Data = summary,
            Message = "Attendance summary retrieved successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<decimal> GetOverallAttendanceRateAsync(CancellationToken cancellationToken)
    {
        var totalEmployees = await repository.CountAsync(cancellationToken);
        if (totalEmployees == 0)
            return 0;
        var today = DateTime.UtcNow.Date;
        var all = await repository.GetAllSummaryAsync(cancellationToken);
        var present = all.Count(e=>e.Date == today && e.CheckIn != null);
        return  Math.Round((decimal)present / totalEmployees * 100, 2);


    } 

    public async Task<List<DepartmentAttendance>> GetDepartmentAttendanceAsync(CancellationToken cancellationToken)
    {
       var today = DateTime.UtcNow.Date;
       var records = await repository.GetAllSummaryAsync(cancellationToken);
       
       var grouped = records
           .Where(x=>x.Employee != null)
           .GroupBy(x=>x.Employee.Department?.Name)
           .Select(g=>new DepartmentAttendance
               {
                   DepartmentName = g.Key ?? "Unknown",
                   EmployeeCount = g.Count(),
                   PresentCount = Math.Round((decimal)g.Count(x=>x.CheckIn != null) / g.Count() * 100, 2 )
               }
               ).ToList();
       return grouped;
    }

    public async Task<List<Activity>> GetRecentActivityAsync(CancellationToken cancellationToken, int count = 10)
    {
       var records = await repository.GetAllSummaryAsync(cancellationToken);
       var activities = records.OrderByDescending(x=>x.UpdatedAt ?? x.CreatedAt).Take(count).Select(x=>new Activity
       {
           EmployeeId = x.EmployeeId,
           EmployeeName = $"{x.Employee?.Title} {x.Employee?.FirstName} {x.Employee?.Surname},",
           Timestamp = x.UpdatedAt ?? x.CreatedAt,
           Action = x.CheckOut != null ? "Cheked Out" : "Checked In",
           Status = x.IsLate ? "Late" : "On Time"
       }).ToList();
       return activities;
    }
    

    public async Task<ApiResponse<BulkAttendanceResponseDto>> BulkAttendanceAsync(BulkAttendanceRequestDto request,
        CancellationToken cancellationToken)
    {
        
        var result = new BulkAttendanceResponseDto();
        var errors = new List<string>();

        if (request.Records.Count==0)
        {
            return new ApiResponse<BulkAttendanceResponseDto>()
            {
                Message = "No Attendance records provided",
                StatusCode = StatusCodes.Status400BadRequest,
                Data = result
            };

        }

        foreach (var record in request.Records)
        {
            try
            {
                var existingRecord =
                    await repository.GetByDateAsync(record.EmployeeId, request.Date, cancellationToken);
                if (existingRecord != null)
                {
                    if (record.CheckIn && existingRecord.CheckOut != null)
                    {
                        result.Failed++;
                        errors.Add($"Employee {record.EmployeeId} already checked in on {request.Date.Date}");
                        continue;
                    }

                    if (record.CheckOut && existingRecord.CheckOut != null)
                    {
                        result.Failed++;
                        errors.Add($"Employee {record.EmployeeId} already checked out on {request.Date.Date}");
                        continue;
                    }


                    var checkInTime = record.CheckInTime ?? request.Date.Date.AddHours(8);
                    var checkOutTime = record.CheckOutTime ?? request.Date.Date.AddHours(17);

                    var attendanceRecord = new AttendanceRecord()
                    {
                        EmployeeId = record.EmployeeId,
                        Date = request.Date,
                        CheckIn = record.CheckIn ? checkInTime : null,
                        CheckOut = record.CheckOut ? checkOutTime : null,
                        IsLate = record.CheckIn && checkInTime > request.Date.Date.AddHours(8),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    var response = await repository.CheckIn(attendanceRecord, cancellationToken);
                    if (response > 0)
                    {
                        result.Successful++;
                    }
                    else
                    {
                        result.Failed++;
                        errors.Add(($"Failed to update attendance for employee {record.EmployeeId}"));
                    }

                    
                }
                bool updated = false;
                
                if (record.CheckIn  && existingRecord.CheckIn == null)
                {
                    existingRecord.CheckIn = record.CheckInTime ?? request.Date.Date.AddHours(8);
                    existingRecord.IsLate = existingRecord.CheckIn > request.Date.Date.AddHours(8);
                    updated = true;
                }

                if (record.CheckOut)
                {
                    if (existingRecord != null)
                        existingRecord.CheckOut = record.CheckOutTime ?? request.Date.Date.AddHours(17);
                    updated = true;
                }

                if (updated)
                {
                    if (existingRecord != null)
                    {
                        existingRecord.UpdatedAt = DateTime.UtcNow;
                        var data = await repository.UpdateAsync(existingRecord, cancellationToken);

                        if (data > 0)
                        {
                            result.Successful++;
                        }
                        else
                        {
                            result.Failed++;
                            errors.Add(($"Failed to update attendance for employee {record.EmployeeId}"));  
                        }
                    }
                    
                    
                }
                else
                {
                    result.Successful++;
                }
                
                

               
            }
            
            catch (System.Exception e)
            {
                logger.LogError($"Error processing employee attendance: {record.EmployeeId}");
            }

            result.TotalProcessed++;
        }
        result.Errors = errors;

        return new ApiResponse<BulkAttendanceResponseDto>()
        {
            Data = result,
            Message = result.Failed == 0 ? "Bulk Attendance process Successfully"
                : $"Bulk Attendance completed with {result.Failed} Failures",
            StatusCode = result.Failed == 0 ? StatusCodes.Status200OK :
                StatusCodes.Status207MultiStatus,
            
        };
    }
}
