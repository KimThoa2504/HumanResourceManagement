namespace HumanResourceManagement.Models.DTOs.Recruitments
{
    public class RecruitmentQuery
    {
        public string? Keyword { get; set; }

        public string? Status { get; set; }

        public string? Department { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
