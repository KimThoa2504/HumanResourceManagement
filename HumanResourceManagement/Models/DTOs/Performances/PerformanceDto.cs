namespace HumanResourceManagement.Models.DTOs.Performances
{
    public class PerformanceDto
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; } = "";

        public int Score { get; set; }

        public string? Review { get; set; }

        public string? Period { get; set; }
    }
}
