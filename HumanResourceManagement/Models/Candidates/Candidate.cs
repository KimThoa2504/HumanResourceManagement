using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceManagement.Models.Candidates
{
    [Table("candidates")]
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = "";

        public string? Email { get; set; }

        public string? Phone { get; set; }

        [Column("recruitment_id")]
        public int? RecruitmentId { get; set; }

        public string Status { get; set; } = "APPLIED";
    }
}

