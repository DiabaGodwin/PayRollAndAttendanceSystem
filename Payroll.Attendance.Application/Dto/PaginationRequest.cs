namespace Payroll.Attendance.Application.Dto;

public class PaginationRequest 
{
    public string? SearchText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set;  }
}

public interface IEmployeeIdFilter
{
    public int? EmployeeId { get; set; }
}