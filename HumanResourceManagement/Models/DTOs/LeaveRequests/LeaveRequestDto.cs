namespace HumanResourceManagement.Models.DTOs.LeaveRequests
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; } = "";

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }

        public string Status { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? AttachmentPath { get; set; }
        public string? AttachmentName { get; set; }
    }
}
