using System.Diagnostics;

namespace Payroll.Attendance.Domain.Models
{
    


public class DashboardSummary
{
    public int TotalEmployees { get; set; }
    public int PendingReminders { get; set; }
    public int AttendanceRate { get; set; }
    public decimal TotalPayroll { get; set; }

}

public  class KeyMetric
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? Change { get; set; }
    public string? ChangeDescription { get; set; }
    
}

public  class DepartmentAttendance
{
    public string DepartmentName { get; set; } = null!;
    public int EmployeeCount { get; set; }
    public decimal PresentCount { get; set; }
}
public class Reminder
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public DateTime DueDate { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    
}
public  class Activity
 {
     
     public int Id { get; set; }
     public int EmployeeId { get; set; }
     public int RecordId { get; set; }
     public string Module {get; set; }
     public string Description { get; set; }
     public string EmployeeName { get; set; }
     public string Action { get; set; }
     public DateTime Timestamp { get; set; }
     
 }

public  class PayrollTrend
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
}


public class DepartmentDistribution
{
    public string DepartmentName { get; set; }
    public string? SalaryAmount { get; set; } 
    public decimal Percentage { get; set; }
    public int  Count { get; set; }
    public decimal TotalAmount { get; set; }
}


}

