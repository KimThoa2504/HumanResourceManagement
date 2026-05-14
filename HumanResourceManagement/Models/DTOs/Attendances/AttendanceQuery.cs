namespace HumanResourceManagement.Models.DTOs.Attendances
{
    public class AttendanceQuery
    {
        public int? EmployeeId { get; set; }

        public DateTime? WorkDate { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
