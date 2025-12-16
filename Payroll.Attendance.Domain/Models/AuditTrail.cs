using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Attendance.Domain.Models
{
    public class AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; } = null!;

        public string Descriptions { get; set; } = string.Empty;

        // Foreign key (UserId)
        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key (UserId)
        public int? UpdatedBy { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public User? UpdatedByUser { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}