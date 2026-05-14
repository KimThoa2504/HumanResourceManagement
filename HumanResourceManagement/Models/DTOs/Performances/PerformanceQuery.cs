namespace HumanResourceManagement.Models.DTOs.Performances
{
    public class PerformanceQuery
    {
        public int? EmployeeId { get; set; }

        public int? ScoreMin { get; set; }

        public int? ScoreMax { get; set; }

        public string? Period { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
