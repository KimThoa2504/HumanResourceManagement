namespace HumanResourceManagement.Models.DTOs.Candidates
{
    public class CreateCandidateDto
    {
        public string FullName { get; set; } = "";

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int RecruitmentId { get; set; }
    }
}
