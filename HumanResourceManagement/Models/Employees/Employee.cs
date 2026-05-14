using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceManagement.Models.Employees
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = "";

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        //public string? Department { get; set; }

        public string? Position { get; set; }

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        public decimal? Salary { get; set; }

        [Column("department_id")]
        public int? DepartmentId { get; set; }

        public string Status { get; set; } = "WORKING";

    }
}
