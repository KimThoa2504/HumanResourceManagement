using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceManagement.Models.Recruitments
{
    [Table("recruitment")]
    public class Recruitment
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Department { get; set; }

        public string Status { get; set; } = "OPEN";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
