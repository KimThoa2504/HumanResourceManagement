using System.ComponentModel.DataAnnotations;

namespace HumanResourceManagement.Models.DTOs.Employees
{
    public class CreateEmployeeDto : BaseEmployeeDto
    {
        [Required]
        public new string FullName { get; set; }

        [Required]
        public new int DepartmentId { get; set; }
    }
}
