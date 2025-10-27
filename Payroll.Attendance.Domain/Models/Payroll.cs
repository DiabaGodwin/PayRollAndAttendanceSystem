namespace Payroll.Attendance.Domain.Models
{
    public class PayrollRecord
    {
        public int Id { get; private set; }
        public int EmployeeId { get; private set; }
        public DateOnly PeriodStart { get; private set; }
        public DateOnly PeriodEnd { get; private set; }
        public decimal BasicSalary { get; private set; }
        public decimal Allowance { get; private set; }
        public decimal Deductions { get; private set; }
        public decimal NetSalary { get; private set; }
        public string Remarks { get; private set; }
        

        public Employee? Employee { get; private set; }

        private PayrollRecord() { } // EF Core requirement

        public PayrollRecord(int employeeId, DateOnly start, DateOnly end,
            decimal basicSalary, decimal allowance, decimal deductions = 0)
        {
            EmployeeId = employeeId;
            PeriodStart = start;
            PeriodEnd = end;
            BasicSalary = basicSalary;
            Allowance = allowance;
            Deductions = deductions;
            NetSalary = CalculateNetSalary();
        }

        private decimal CalculateNetSalary()
        {
            return (BasicSalary + Allowance) - Deductions;
        }

        public void ApplyDeductions(decimal amount, string reason)
        {
            Deductions += amount;
            NetSalary = CalculateNetSalary();
            Remarks = reason;
        }

        public void RecalculateNetSalary()
        {
            NetSalary = CalculateNetSalary();
        }
    }
}