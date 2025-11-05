namespace Payroll.Attendance.Application.Dto;

public class DepartmentResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
    public List<DepartmentEmployeeDto>? Employees { get; set; }
}