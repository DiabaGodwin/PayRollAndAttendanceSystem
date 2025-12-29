using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Payroll.Attendance.Domain.Models
{
    

//not important
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
    public string? Description {  get; set; }
    [Required]
    public string? Priority { get; set; }
    
    public DateTime DueDate { get; set; }
   [Required]
    public int DepartmentId { get; set; }
   
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    
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

