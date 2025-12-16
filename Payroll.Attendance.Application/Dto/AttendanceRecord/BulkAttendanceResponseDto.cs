namespace Payroll.Attendance.Application.Dto.AttendanceRecord;

public class BulkAttendanceResponseDto
{
 public int TotalProcessed { get; set; }   
 public int Successful { get; set; }
 public int Failed { get; set; }
 public List<string> Errors { get; set; }

    
}