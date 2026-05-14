namespace HumanResourceManagement.Models.DTOs.Recruitments
{
    public class RecruitmentDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Department { get; set; }

        public string Status { get; set; } = "";
    }
}
