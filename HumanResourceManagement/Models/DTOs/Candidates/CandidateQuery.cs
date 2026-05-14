namespace HumanResourceManagement.Models.DTOs.Candidates
{
    public class CandidateQuery
    {
        public string? Keyword { get; set; }

        public string? Status { get; set; }

        public int? RecruitmentId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
