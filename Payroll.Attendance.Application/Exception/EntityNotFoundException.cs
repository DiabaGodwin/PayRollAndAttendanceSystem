namespace Payroll.Attendance.Application.Exception;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string entity, string id)
        : base($"{entity} with ID '{id}' was not found.") { }
}