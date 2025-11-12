using System.Text.Json.Serialization;

namespace Payroll.Attendance.Application.Dto.Employee;

public class GetOnlyDepartmentDto
{
      [JsonPropertyName("dptId")]
      public int Id { get; set; }
      public string Name { get; set; }
}