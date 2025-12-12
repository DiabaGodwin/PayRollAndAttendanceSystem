using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.AttendanceRecord;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Enum;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class AttendanceService(IAttendanceRepository repository,  ILogger<AttendanceService> logger, IEmployeeRepository employeeRepository) : IAttendanceService
{
    public async Task<ApiResponse<int>> CheckIn( AttendanceRequest request,  CancellationToken cancellationToken)
    {
        var existingRecord =
            await repository.CheckIfAttendanceExistAsync(request.EmployeeId, 
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

        result.FirstName = res.Employee.FirstName;
        result.Surname = res.Employee.Surname;

        return new ApiResponse<AttendanceResponseDto>()
        {
            Message = "Your Request was successfull",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        }; 

    }

    public async Task<ApiResponse<int>> CheckOutAsync(UpdatedAttendanceRequest request, CancellationToken cancellationToken)
    {
         
       
       var existingRecord = await repository.CheckIfAttendanceExistAsync(request.EmployeeId, DateTime.UtcNow.Date, cancellationToken);
       
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

    public async Task<ApiResponse<AttendanceSummaryDto>> GetAllSummaryAsync( CancellationToken cancellationToken)
          {
       var employees = await employeeRepository.GetAllEmployeesAsync(new PaginationRequest{PageNumber = 1, PageSize = 1000}, cancellationToken);
       var currentEmployeeeIds = employees.Select(e => e.Id).ToHashSet();
       
       var totalEmployees =  currentEmployeeeIds.Count;
      
        
      
        
        var today = DateTime.UtcNow.Date;
        var lateThreshhold = new TimeSpan(8, 0, 0);
        
        
        var allRecords = await repository.GetAllSummaryAsync(cancellationToken);
        
        var currentEmployeeRecords = allRecords.Where(e=>currentEmployeeeIds.Contains(e.EmployeeId)).ToList();
       
        //Daily Today's Summary
        var recordsToday = allRecords.Where(x=>x.Date.Date == today.Date ).ToList();
        var presentToday = recordsToday.Where(e => e.CheckIn.HasValue).Select(x=>x.EmployeeId).Distinct().Count();
        var lateArrivals = recordsToday.Where(e=>e.CheckIn.HasValue && e.CheckIn.Value.TimeOfDay > lateThreshhold).Select(x=>x.EmployeeId).Distinct().Count();
        var onLeaveToday = recordsToday.Where(x=>x.IsOnLeave == true).Select(x=>x.EmployeeId).Distinct().Count();
        var absentToday  = totalEmployees - (onLeaveToday + presentToday);
        if(absentToday < 0) absentToday = 0;
        double attendancePercentage = totalEmployees == 0 ? 0
            : Math.Round(((double)presentToday / totalEmployees) * 100, 1);

        var employeeIds = allRecords.Select(r => r.EmployeeId).Distinct().ToHashSet();
        
        //WEEKLY SUMMARY
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
        var endOfWeek = startOfWeek.AddDays(6);
        var weekRecords = currentEmployeeRecords.Where(v=>v.Date >= startOfWeek.Date && v.Date <= endOfWeek).ToList();
        var workingDaysWeek = weekRecords.Select(v=>v.Date.Date).Distinct().Count();
        var weekGroups = weekRecords.GroupBy(v=> new {v.EmployeeId, v.Date.Date}).Select(f=>new
        {
            EmployeeId = f.Key.EmployeeId,
            Day = f.Key.Date,
            Status = f.OrderByDescending(f=>f.Date).First().Status,
        }).ToList();
        var overallPresentWeek = weekGroups.Count(r => r.Status == AttendanceStatus.Present);
        var overallLeaveWeek = weekGroups.Count(r => r.Status == AttendanceStatus.Leave);
        var overallExpectedWeek = totalEmployees * workingDaysWeek;
        var overallAbsentWeek = overallExpectedWeek - (overallLeaveWeek+ overallPresentWeek);
        if(overallAbsentWeek < 0) overallAbsentWeek = 0;
        double averageAttendancePercentageWeek = overallExpectedWeek == 0 ? 0
            : Math.Round(((double)overallPresentWeek / overallExpectedWeek) * 100, 1);
        
        //MONTH SUMMARY
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var monthRecords = currentEmployeeRecords.Where(x=>x.Date>= firstDayOfMonth && x.Date <= endOfMonth).ToList();
        var workingDaysMonth = monthRecords.Select(x => x.Date.Date).Distinct().Count();
        var monthGroups = monthRecords.GroupBy(x=>new {x.EmployeeId, Day = x.Date.Date}).
                Select(d=>new
                {
                    EmployeeId = d.Key.EmployeeId,
                    Day = d.Key.Day,
                    Status = d.OrderByDescending(x=>x.Date).First().Status,
                }).ToList();
        var overallPresentMonth = monthGroups.Count(r => r.Status == AttendanceStatus.Present);
        var overallLeaveMonth = monthGroups.Count(g=>g.Status == AttendanceStatus.Leave);
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
                    OnLeave = onLeaveToday,
                    AttendancePercentage = attendancePercentage
                },
                Weekly = new WeekSummaryDto()
                {
                   PresentWeek = overallPresentWeek,
                   AbsentWeek = overallAbsentWeek,
                   WeekLateArrivals = overallLeaveWeek,
                   WeekOnLeave = overallLeaveWeek,
                   WeeklyAttendancePercentage = averageAttendancePercentageWeek
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
//Recent Activity
    
//Bulk Attendance
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

            var data = new AttendanceRecord()
            {
                EmployeeId = record.EmployeeId,
                Date = request.Date.Date,
                CheckIn = record.CheckInTime.HasValue ? request.Date.Date + record.CheckInTime.Value.TimeOfDay : null,
                CheckOut = record.CheckOutTime.HasValue ? request.Date.Date + record.CheckOutTime.Value.TimeOfDay : null,
                IsLate = record.CheckInTime > DateTime.Today.Date.AddHours(8),
                CreatedAt = DateTime.UtcNow,
            };
            try
            {
                //Checking if the attendance record exist
                var existingRecord =
                    await repository.CheckIfAttendanceExistAsync(data.EmployeeId, request.Date, cancellationToken);
                //Adding new records
                if (existingRecord == null)
                {
                    
                    var response = await repository.CheckIn(data, cancellationToken);
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

                if (existingRecord != null)
                {
                    var update = await repository.UpdateAttendanceAsync(existingRecord, cancellationToken);
                    if (update > 0)
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

    public async Task<ApiResponse<List<AttendanceResponseDto>>> GetTodayAttendanceAsync(
        CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow.Date;
            var records = await repository.GetTodayAttendanceAsync(cancellationToken);
            var response = new List<AttendanceResponseDto>();
            foreach (var r in  records)

            {
                var result = r.Adapt(new AttendanceResponseDto());
                result.FirstName = r.Employee.FirstName;
                result.Surname = r.Employee.Surname;
                result.Department = r.Employee.Department?.Name;
                response.Add(result);
            }
            return new ApiResponse<List<AttendanceResponseDto>>()
            {
                Message = "Today Attendance retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = response
            };
    }

    public async Task<ApiResponse<List<TodayAttendanceDto>>> GetTodayAttendanceWithoutTokenAsync(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow.Date;
        var records = await repository.GetTodayAttendanceWithoutTokenAsync(cancellationToken);
        var response = new List<TodayAttendanceDto>();
        foreach (var r in records)
        {
            var result = r.Adapt(new TodayAttendanceDto());
            result.Title = r.Employee.Title;
            result.FirstName = r.Employee.FirstName;
            result.Surname = r.Employee.Surname;
            result.JobPosition = r.Employee.JobPosition;
            response.Add(result);
        }

        return new ApiResponse<List<TodayAttendanceDto>>()
        {
            Message = "Today Attendance retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = response
        };
    }

    public async Task<ApiResponse<TodayAttendanceSummaryDto>>GetOnlyTodayAttendanceSummaryAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetAllEmployeesAsync(new PaginationRequest{PageNumber = 1, PageSize = 1000}, cancellationToken);
        var currentEmployeeeIds = employees.Select(e => e.Id).ToHashSet();
       
        var totalEmployees =  currentEmployeeeIds.Count;
         var today = startDate.Date;
         var lateThreshold = new TimeSpan(8, 0, 0);

         var allRecords = await repository.GetOnlyTodayAttendanceSummary(startDate, endDate, cancellationToken);
         var currentEmployeeRecords = allRecords.Where(r => currentEmployeeeIds.Contains(r.Id)).ToList();
         var  recordsToday = currentEmployeeRecords.Where(r => r.Date.Date == today.Date).ToList();
         var presentToday = recordsToday.Where(e => e.CheckIn.HasValue).Select(e => e.Employee).Distinct().Count();
         var lateArrivals = recordsToday.Where(e => e.CheckIn.HasValue && e.CheckIn.Value.TimeOfDay > lateThreshold)
             .Select(e => e.Employee).Distinct().Count();
         var onLeaveToday = recordsToday.Where(x=>x.IsOnLeave).Select(x => x.Employee).Distinct().Count();
         var absentToday = totalEmployees - (presentToday + onLeaveToday);
         
         if(absentToday < 0) absentToday = 0;
         double attendancePercentage = totalEmployees == 0 ? 0
             : Math.Round((double)absentToday / totalEmployees * 100, 1);

         var todaySummary = new TodayAttendanceSummaryDto()
         {
             TotalEmployee = totalEmployees,
             PresentToday = presentToday,
             AbsentToday = absentToday,
             LateArrivals = lateArrivals,
             OnLeave = onLeaveToday,
             AttendancePercentage = attendancePercentage
         };
         return new ApiResponse<TodayAttendanceSummaryDto>()
         {
             Message = "Today attendance retrieved successfully",
             StatusCode = StatusCodes.Status200OK,
             Data = todaySummary
         };




    }

    public async Task<ApiResponse<WeekAttendanceSummaryDto>> GetOnlyWeekAttendanceSummaryAsync(DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<MonthAttendanceSummaryDto>> GetOnlyMonthAttendanceSummaryAsync(DateTime startDate,
        DateTime endDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
