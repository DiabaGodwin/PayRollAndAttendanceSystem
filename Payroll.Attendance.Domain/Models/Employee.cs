using Payroll.Attendance.Domain.Enum;


namespace Payroll.Attendance.Domain.Models
{
    public class Employee
    {
        public int Id { get;  set; } 
        public string? FullName { get; set; } = null!;
        public EmployeeCategory Category { get;  set; }
        public string Department { get;  set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get;  set; }
        public decimal TopUp { get; set; }
        public decimal Deduction { get; set; }

       
        private Employee() { }

        public Employee(string fullName, EmployeeCategory category, string department,
            decimal basicSalary, decimal allowance, decimal topUp, decimal deduction)
        {
            FullName = fullName;
            Category = category;
            Department = department;
            BasicSalary = basicSalary;
            Allowance = allowance;
            TopUp = topUp;
            Deduction = deduction;
        }

        public Employee(string dtoFullName, EmployeeCategory dtoCategory, string dtoDepartment, decimal dtoBasicSalary, decimal dtoAllowance, decimal dtoTopUp)
        {
            throw new NotImplementedException();
        }

        public decimal CalculateNetSalary()
        {
            return Category switch
            {
                EmployeeCategory.Fulltime=>BasicSalary + Allowance - Deduction,
                EmployeeCategory.PartTime => BasicSalary + Allowance - Deduction,
                EmployeeCategory.Nss => 559.00m + TopUp,
                EmployeeCategory.Intern => Allowance,
                _ => 0
            };
        }

       
        public void ChangeDepartment(string newDepartment)
        {
            if (string.IsNullOrWhiteSpace(newDepartment))
                throw new ArgumentException("Department cannot be empty.");

            Department = newDepartment;
        }

        public void UpdateAllowance(decimal newAllowance)
        {
            if (newAllowance < 0)
                throw new ArgumentException();

            Allowance = newAllowance;
        }
    }
}