using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.Performances;
using HumanResourceManagement.Models.Performances;

namespace HumanResourceManagement.Services.Performances
{
    public class PerformanceService
    {
        private readonly AppDbContext _context;
        public PerformanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PerformanceDto> Create(CreatePerformanceDto dto)
        {
            if (dto.Score < 0 || dto.Score > 100)
                throw new ApiException("Score must be between 0 and 100");

            var emp = _context.Employees
                .FirstOrDefault(x => x.Id == dto.EmployeeId);

            if (emp == null)
                throw new ApiException("Employee not found");

            var performance = new Performance
            {
                EmployeeId = dto.EmployeeId,
                Score = dto.Score,
                Review = dto.Review,
                Period = dto.Period
            };

            _context.Performances.Add(performance);

            await _context.SaveChangesAsync();

            return new PerformanceDto
            {
                Id = performance.Id,
                EmployeeName = emp.FullName,
                Score = performance.Score,
                Review = performance.Review,
                Period = performance.Period
            };
        }

        public List<PerformanceDto> GetAll()
        {
            var employees = _context.Employees.ToList();

            return _context.Performances
                .ToList()
                .Select(p => new PerformanceDto
                {
                    Id = p.Id,
                    EmployeeName = employees
                        .FirstOrDefault(e => e.Id == p.EmployeeId)?.FullName ?? "",
                    Score = p.Score,
                    Review = p.Review,
                    Period = p.Period
                }).ToList();
        }

        public object Search(PerformanceQuery query)
        {
            var data = _context.Performances.AsQueryable();

            // employee
            if (query.EmployeeId.HasValue)
            {
                data = data.Where(x =>
                    x.EmployeeId == query.EmployeeId);
            }

            // score min
            if (query.ScoreMin.HasValue)
            {
                data = data.Where(x =>
                    x.Score >= query.ScoreMin);
            }

            // score max
            if (query.ScoreMax.HasValue)
            {
                data = data.Where(x =>
                    x.Score <= query.ScoreMax);
            }

            // period
            if (!string.IsNullOrEmpty(query.Period))
            {
                data = data.Where(x =>
                    x.Period.Contains(query.Period));
            }

            var total = data.Count();

            var employees = _context.Employees.ToList();

            var result = data
                .OrderByDescending(x => x.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Score,
                    x.Review,
                    x.Period,

                    Employee = employees
                        .FirstOrDefault(e => e.Id == x.EmployeeId)?.FullName
                });

            return new
            {
                total,
                page = query.Page,
                pageSize = query.PageSize,
                data = result
            };
        }
    }
}
