namespace Payroll.Attendance.Application.Exception;

public class IllegalOperationException : ApplicationException
{
    public IllegalOperationException(string message) : base(message)
    {
    }
}