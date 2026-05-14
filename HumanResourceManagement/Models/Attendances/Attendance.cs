using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceManagement.Models.Attendances
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("work_date")]
        public DateTime WorkDate { get; set; }

        [Column("check_in")]
        public TimeSpan? CheckIn { get; set; }

        [Column("check_out")]
        public TimeSpan? CheckOut { get; set; }
    }
}
