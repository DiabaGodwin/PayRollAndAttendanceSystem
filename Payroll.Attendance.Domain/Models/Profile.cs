using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Attendance.Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DOB {  get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public User User { get; set; }
    }
}
