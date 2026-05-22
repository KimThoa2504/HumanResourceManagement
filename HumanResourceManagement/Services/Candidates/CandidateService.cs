using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.Candidates;
using HumanResourceManagement.Models.DTOs.Candidates;

namespace HumanResourceManagement.Services.Candidates
{
    public class CandidateService
    {
        private readonly AppDbContext _context;

        public CandidateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CandidateDto> Create(CreateCandidateDto dto)
        {
            var recruitment = _context.Recruitments
                .FirstOrDefault(x => x.Id == dto.RecruitmentId);

            if (recruitment == null)
                throw new ApiException("Recruitment not found");

            var candidate = new Candidate
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                RecruitmentId = dto.RecruitmentId
            };

            _context.Candidates.Add(candidate);

            await _context.SaveChangesAsync();

            return new CandidateDto
            {
                Id = candidate.Id,
                FullName = candidate.FullName,
                Email = candidate.Email,
                RecruitmentTitle = recruitment.Title ?? "",
                Status = candidate.Status
            };
        }

        public List<CandidateDto> GetAll()
        {
            var recruitments = _context.Recruitments.ToList();

            return _context.Candidates
                .ToList()
                .Select(c => new CandidateDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Phone = c.Phone,
                    RecruitmentTitle = recruitments
                        .FirstOrDefault(r => r.Id == c.RecruitmentId)?.Title ?? "",
                    Status = c.Status
                }).ToList();
        }

        public async Task UpdateStatus(int id, UpdateCandidateStatusDto dto)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
                throw new ApiException("Candidate not found");

            candidate.Status = dto.Status;

            await _context.SaveChangesAsync();
        }

        public object Search(CandidateQuery query)
        {
            var data = _context.Candidates.AsQueryable();

            // search name
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                data = data.Where(x =>
                    x.FullName.Contains(query.Keyword));
            }

            // filter status
            if (!string.IsNullOrEmpty(query.Status))
            {
                data = data.Where(x =>
                    x.Status == query.Status);
            }

            // filter recruitment
            if (query.RecruitmentId.HasValue)
            {
                data = data.Where(x =>
                    x.RecruitmentId == query.RecruitmentId);
            }

            var total = data.Count();

            var recruitments = _context.Recruitments.ToList();

            var result = data
                .OrderByDescending(x => x.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Phone,
                    x.Status,
                    Recruitment = recruitments
                        .FirstOrDefault(r => r.Id == x.RecruitmentId)?.Title
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
