using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.Recruitments;
using HumanResourceManagement.Models.Recruitments;

namespace HumanResourceManagement.Services.Recruitments
{
    public class RecruitmentService
    {
        private readonly AppDbContext _context;

        public RecruitmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentDto> Create(CreateRecruitmentDto dto)
        {
            var recruitment = new Recruitment
            {
                Title = dto.Title,
                Description = dto.Description,
                Department = dto.Department
            };

            _context.Recruitments.Add(recruitment);

            await _context.SaveChangesAsync();

            return new RecruitmentDto
            {
                Id = recruitment.Id,
                Title = recruitment.Title,
                Department = recruitment.Department,
                Status = recruitment.Status
            };
        }

        public List<RecruitmentDto> GetAll()
        {
            return _context.Recruitments
                .Select(x => new RecruitmentDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Department = x.Department,
                    Status = x.Status
                }).ToList();
        }

        public async Task Close(int id)
        {
            var recruitment = await _context.Recruitments.FindAsync(id);

            if (recruitment == null)
                throw new ApiException("Recruitment not found");

            recruitment.Status = "CLOSED";

            await _context.SaveChangesAsync();
        }

        public object Search(RecruitmentQuery query)
        {
            var data = _context.Recruitments.AsQueryable();

            // search title
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                data = data.Where(x =>
                    x.Title.Contains(query.Keyword));
            }

            // filter status
            if (!string.IsNullOrEmpty(query.Status))
            {
                data = data.Where(x =>
                    x.Status == query.Status);
            }

            // filter department
            if (!string.IsNullOrEmpty(query.Department))
            {
                data = data.Where(x =>
                    x.Department.Contains(query.Department));
            }

            var total = data.Count();

            var result = data
                .OrderByDescending(x => x.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

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
