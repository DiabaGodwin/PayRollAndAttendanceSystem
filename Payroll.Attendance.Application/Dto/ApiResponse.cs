namespace Payroll.Attendance.Application.Dto;

public class ApiResponse<T>
{
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public required int StatusCode { get; set; }
}