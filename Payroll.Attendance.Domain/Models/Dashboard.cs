using System.Diagnostics;

namespace Payroll.Attendance.Domain.Models
{
    


public class DashboardSummary
{
    public string WelcomeMessage { get; set; } = null!;
    public List<KeyMetric> KeyMetrics { get; set; } = new();
    public List<DepartmentAttendance> DepartmentAttendances { get; set; } = new();
    public List<Reminder> Reminders { get; set; } = new();
    public List<Activity> Activities { get; set; } = new();
    public List<PayrollTrend> PayrollTrends { get; set; } = new();
    public List<DepartmentDistribution> DepartmentDistributions { get; set; } = new();

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
    public string? DepartmentName { get; set; }
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
    public int EmployeeId {get; set;}
    public string? EmployeeName { get; set; }
    public string? Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string? TimeDisplay { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }    
}

public  class PayrollTrend
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
}


public class DepartmentDistribution
{
    public Department? DepartmentName { get; set; }
    public string? SalaryAmount { get; set; } 
    public decimal Percentage { get; set; }
    public int  Count { get; set; }
    public decimal TotalAmount { get; set; }
}


}

