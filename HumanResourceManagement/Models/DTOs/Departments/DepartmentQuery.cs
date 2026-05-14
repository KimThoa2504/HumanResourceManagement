namespace HumanResourceManagement.Models.DTOs.Departments
{
    public class DepartmentQuery
    {
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
