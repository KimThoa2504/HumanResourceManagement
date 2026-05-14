namespace HumanResourceManagement.Models.DTOs.Employees
{
    public class EmployeeQuery
    {
        public string? Keyword { get; set; }
        public string? DepartmentId { get; set; }
        public string? Status { get; set; }


        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
