namespace HumanResourceManagement.Models.DTOs.Employees
{
    public class BaseEmployeeDto
    {
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DepartmentId { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
    }
}
