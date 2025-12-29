namespace Payroll.Attendance.Application.Dto.Employee;

public class GetEmployeeRequest : PaginationRequest
{
   public int EmployeeId { get; set; } 
}