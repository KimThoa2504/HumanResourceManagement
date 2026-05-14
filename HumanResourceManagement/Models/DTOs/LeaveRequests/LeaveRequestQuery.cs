namespace HumanResourceManagement.Models.DTOs.LeaveRequests
{
    public class LeaveRequestQuery
    {
        public int? EmployeeId { get; set; }

        public string? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

}
