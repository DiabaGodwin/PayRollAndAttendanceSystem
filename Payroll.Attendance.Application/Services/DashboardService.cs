using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Dto.DashBoard;
using Payroll.Attendance.Application.Repositories;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Application.Services;

public class DashboardService(IDashboardRepository dashboardRepository, IEmployeeRepository employeeRepository, IAuditTrailRepo auditTrailRepo) : IDashboardService

{
    public async Task<ApiResponse<DashBoardSummaryDto>> GetDashboardSummaryAsync(CancellationToken cancellationToken)
    { var employeesResult = await employeeRepository.GetAllEmployeesAsync(new PaginationRequest{PageSize = 10000, PageNumber = 1}, cancellationToken); 
        var summary = new DashBoardSummaryDto()
        {
            TotalEmployees = employeesResult.Count,
            PendingReminders = await dashboardRepository.GetPendingReminderCountAsync(cancellationToken),
            TotalPayroll = await dashboardRepository.GetTotalPayrollAsync(cancellationToken ),
            AttendanceRate = await dashboardRepository.GetpresentCountAsync(cancellationToken)
            
        };
        var audit = new AuditTrail
        {
            Action = "Dashboard summary viewed",
            Descriptions = $"Dashboard summary viewed by {employeesResult.Count} employees",
        };
        
        return new ApiResponse<DashBoardSummaryDto>()
        {
            Message = "Dashboard Summary retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = summary
        };
    }

    public async Task<ApiResponse<List<PayrollTrendDto>>> GetPayrollTrendAsync(CancellationToken cancellationToken)
    {
       
        var trends = await dashboardRepository.GetPayrollTrendsAsync(cancellationToken);
        var response = trends.Select(x=> new PayrollTrendDto
        {
            Month = x.Month,
            Amount = x.Amount,
        }).ToList();
        return new ApiResponse<List<PayrollTrendDto>>()
        {
            Message = "Payroll Trends retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = response
        };

    }

    public async Task<ApiResponse<List<DepartmentDistributionDto>>> GetDepartmentDistributionAsync(
        CancellationToken cancellationToken)
    {
        var results = await dashboardRepository.GetDepartmentDistributionsAsync(cancellationToken);
        var response = results.Select(d=>new DepartmentDistributionDto
        {
            DepartmentName = d.DepartmentName,
            Count = d.Count,
        }).ToList();
        return new ApiResponse<List<DepartmentDistributionDto>>()
        {
            Message = "Department Distributions retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = response
        };
    }

    public async Task<ApiResponse<List<ReminderDto>>> CreatReminderAsync(CreateReminderRequest request,
        CancellationToken cancellationToken)
    {
        var newReminder = new Reminder
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            IsCompleted = false
        };
        await dashboardRepository.AddReminderAsync(newReminder, cancellationToken);
        var reminders = await dashboardRepository.GetPendingRemindersAsync(cancellationToken);

        var response = reminders.Select(e => new ReminderDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Priority = e.Priority,
            DueDate = e.DueDate,
            IsCompleted = e.IsCompleted
        }).ToList();
        return new ApiResponse<List<ReminderDto>>()
        {
            Message = "Reminders retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = response

        };
    }

    public async Task<ApiResponse<int>> DeleteReminderAsync(int id, CancellationToken cancellationToken)
    {
         await dashboardRepository.DeleteReminderAsync(id,
            cancellationToken);

         return new ApiResponse<int>()
         {
             Message = "Reminder deleted successfully",
             StatusCode = StatusCodes.Status200OK
         };

    }

    public async Task<ApiResponse<FullDashboardResponseDto>> GetFullDashboardAsync(CancellationToken cancellationToken)
    { 
        var summary = await GetDashboardSummaryAsync(cancellationToken);
        var reminders = await dashboardRepository.GetPendingRemindersAsync(cancellationToken);
        var department = await dashboardRepository.GetDepartmentDistributionsAsync(cancellationToken);
        var payrollTrends = await dashboardRepository.GetPayrollTrendsAsync(cancellationToken);

        var response = new FullDashboardResponseDto
        {
            Summary = summary.Data,
            Reminders = reminders.Select(r => new ReminderDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                DueDate = r.DueDate,
                Priority = r.Priority,
                IsCompleted = r.IsCompleted
            }).ToList(),
            DepartmentDistribution = department.Select(j => new DepartmentDistributionDto
            {
                DepartmentName = j.DepartmentName,
                Count = j.Count
            }).ToList(),
            PayrollTrend = payrollTrends.Select(k => new PayrollTrendDto
            {
                Month = k.Month,
                Amount = k.Amount,
            }).ToList()
        };
        return new ApiResponse<FullDashboardResponseDto>()
        {
            Message = "Full dashboard retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = response
        };
    }

    public async Task<ApiResponse<List<ActivityDto>>> GetActivityAsync(CancellationToken cancellationToken)
    {
        var employees =
            await employeeRepository.GetAllEmployeesAsync(new PaginationRequest { PageNumber = 1, PageSize = 10000 },
                cancellationToken);
        var employeeLookup = employees
            .GroupBy(x => x.EmployeeId)
            .ToDictionary(f => f.Key, f => $"{f.First().Title} {f.First().FirstName} {f.First().Surname}");


        List<ActivityDto> activities = new();

        var attendance = await dashboardRepository.GetAllSummaryAsync(cancellationToken);
        activities.AddRange(attendance.Select(x=> new ActivityDto
        {
            RecordId = x.Id,
            Module = "Attendance",
            Action = x.CheckOut != null ? "Check out" : "Check In",
            Description = $"{employeeLookup.GetValueOrDefault(x.EmployeeId)} {(x.CheckOut != null ? "check out" : "Check In")}",
            EmployeeName = employeeLookup.GetValueOrDefault(0),
            Timestamp = x.UpdatedAt ?? x.CreatedAt
        }));
        
        var payrolls = await dashboardRepository.GetAllPayrollAsync(cancellationToken);
        activities.AddRange(payrolls.Select(x=> new ActivityDto
        {
          RecordId  = x.Id,
          Module = "Payroll",
          Action = "Payroll Processed",
          Description = $"Payroll Processed for {employeeLookup.GetValueOrDefault(x.EmployeeId)}",
          Timestamp = x.UpdatedAt 
        }));

        var departments = await dashboardRepository.GetAllDepartmentsAsync(cancellationToken);
        activities.AddRange(departments.Select(x=>new ActivityDto
        {
            RecordId = x.Id,
            Module = "Department",
            Action = "Department Created",
            Description = $"Department '{x.Name}' was created",
            EmployeeName = "system", 
            Timestamp = x.UpdatedAt ?? x.CreatedAt
        }));
        var result = activities.OrderByDescending(c=>c.Timestamp).Take(30).ToList();

        return new ApiResponse<List<ActivityDto>>()
        {
            Message = "Recent Activities retrived successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = result
        };
    } 
   
}