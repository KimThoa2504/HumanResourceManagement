namespace HumanResourceManagement.Models.DTOs.Candidates
{
    public class CandidateDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string? Email { get; set; }

        public string RecruitmentTitle { get; set; } = "";

        public string Status { get; set; } = "";
    }
}
