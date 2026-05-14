using HumanResourceManagement.Data;

namespace HumanResourceManagement.Services.Dashboards
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public object GetDashboard()
        {
            var totalEmployees = _context.Employees.Count();

            var workingEmployees = _context.Employees
                .Count(x => x.Status == "WORKING");

            var totalDepartments = _context.Departments.Count();

            var totalRecruitments = _context.Recruitments
                .Count(x => x.Status == "OPEN");

            var pendingLeaveRequests = _context.LeaveRequests
                .Count(x => x.Status == "Pending");

            var totalCandidates = _context.Candidates.Count();

            var avgPerformance = _context.Performances.Any()
                ? _context.Performances.Average(x => x.Score)
                : 0;

            return new
            {
                totalEmployees,
                workingEmployees,
                totalDepartments,
                openRecruitments = totalRecruitments,
                pendingLeaveRequests,
                totalCandidates,
                averagePerformanceScore = avgPerformance
            };
        }

    }
}
