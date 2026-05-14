namespace HumanResourceManagement.Models.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string Status { get; set; }
    }
}
