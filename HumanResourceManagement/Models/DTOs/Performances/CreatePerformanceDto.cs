namespace HumanResourceManagement.Models.DTOs.Performances
{
    public class CreatePerformanceDto
    {
        public int EmployeeId { get; set; }

        public int Score { get; set; }

        public string? Review { get; set; }

        public string? Period { get; set; }
    }
}
