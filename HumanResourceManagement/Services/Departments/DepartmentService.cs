using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.Departments;
using HumanResourceManagement.Models.DTOs.Departments;

namespace HumanResourceManagement.Services.Departments
{
    public class DepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }
        // CREATE
        public async Task<DepartmentDto> Create(CreateDepartmentDto dto)
        {
            if (_context.Departments.Any(d => d.Name == dto.Name))
                throw new ApiException("Department already exists");

            var dept = new Department
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description
            };
        }

        // geall
        public List<DepartmentDto> GetAll()
        {
            return _context.Departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
        }

        // delete
        public async Task Delete(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null)
                throw new ApiException("Department not found");

            // Check employee đang dùng
            var isUsed = _context.Employees.Any(e => e.DepartmentId == id);
            if (isUsed)
                throw new ApiException("Cannot delete - Department is used by employees");

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
        }

        // search
        public object Search(DepartmentQuery query)
        {
            var data = _context.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                data = data.Where(x =>
                    x.Name.Contains(query.Keyword));
            }

            var total = data.Count();

            var result = data
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

        //update
        public async Task<DepartmentDto> Update(
            int id,
            UpdateDepartmentDto dto
        )
        {
            var dept = await _context.Departments.FindAsync(id);

            if (dept == null)
                throw new ApiException("Department not found");

            // check duplicate
            var existed = _context.Departments.Any(d =>
                d.Name == dto.Name &&
                d.Id != id
            );

            if (existed)
                throw new ApiException("Department already exists");

            dept.Name = dto.Name;
            dept.Description = dto.Description;

            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description
            };
        }
    }
}
