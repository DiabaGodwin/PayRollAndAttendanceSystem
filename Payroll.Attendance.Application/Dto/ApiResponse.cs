namespace Payroll.Attendance.Application.Dto;

public class ApiResponse<T>
{
    public int Id { get; set; }

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    
    public int Data { get; set; }
    public int StatusCode { get; set; }
}