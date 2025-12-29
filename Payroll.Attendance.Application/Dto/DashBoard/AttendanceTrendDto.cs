namespace Payroll.Attendance.Application.Dto.DashBoard;

public class AttendanceTrendDto
{
    public string label { get; set; }
    public int Present { get; set; }
    public int TotalPenalties { get; set; }
    public int Absent { get; set; }
    public int LateArrivals { get; set; }

}