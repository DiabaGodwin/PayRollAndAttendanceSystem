using System.Text.Json.Serialization;
using Payroll.Attendance.Application.Dto.Employee;

namespace Payroll.Attendance.Application.Dto.Department;

public class DepartmentResponseDto
{
    [JsonPropertyName("dptId")]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
    public IEnumerable<EmployeeResponseDto>? Employees { get; set; }
}