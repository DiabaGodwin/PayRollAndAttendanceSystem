namespace Payroll.Attendance.Application.Exception;

public class ValidationException : ApplicationException
{
    public ValidationException(string message) : base(message)
    {
    }
}