using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.Employees;
using HumanResourceManagement.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagement.Services.Employees
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        //Create employee
        public async Task<EmployeeDto> Create(CreateEmployeeDto dto)
        {
            var emp = new Employee
            {
                FullName = dto.FullName,
                Dob = dto.Dob,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Address = dto.Address,
                DepartmentId = dto.DepartmentId,
                Position = dto.Position,
                Salary = dto.Salary
            };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Position = emp.Position,
                Status = emp.Status
            };
        }

        // Get all employees
        public async Task<List<EmployeeDto>> GetAll()
        {
            return await _context.Employees
                .Join(
                    _context.Departments,
                    emp => emp.DepartmentId,
                    dept => dept.Id,
                    (emp, dept) => new EmployeeDto
                    {
                        Id = emp.Id,
                        FullName = emp.FullName,
                        Department = dept.Name,
                        Position = emp.Position,
                        Status = emp.Status
                    }
                )
                .ToListAsync();
        }


        // Get employee by id
        public EmployeeDto GetById(int id)
        {
            var emp = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (emp == null) throw new ApiException("Not found");

            var dept = _context.Departments
                .FirstOrDefault(d => d.Id == emp.DepartmentId);

            return new EmployeeDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Department = dept?.Name,
                Position = emp.Position,
                Status = emp.Status
            };
        }


        //delete employee 
        public async Task Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                throw new ApiException("Not found");

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

        }


        //update employee
        public async Task<EmployeeDto> Update(int id, UpdateEmployeeDto dto)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) throw new ApiException("Not found");

            if (!string.IsNullOrEmpty(dto.FullName)) emp.FullName = dto.FullName;
            if (dto.DepartmentId.HasValue) emp.DepartmentId = dto.DepartmentId;
            if (!string.IsNullOrEmpty(dto.Position)) emp.Position = dto.Position;
            if (dto.Salary.HasValue) emp.Salary = dto.Salary;

            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Position = emp.Position,
                Status = emp.Status
            };
        }

        //search
        public object Search(EmployeeQuery query)
        {
            var data = _context.Employees
                .Join(
                    _context.Departments,
                    emp => emp.DepartmentId,
                    dept => dept.Id,
                    (emp, dept) => new
                    {
                        Employee = emp,
                        DepartmentName = dept.Name
                    }
                )
                .AsQueryable();

            // keyword
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                data = data.Where(x =>
                    x.Employee.FullName.Contains(query.Keyword));
            }

            // search theo tên phòng ban
            if (!string.IsNullOrEmpty(query.DepartmentId))
            {
                data = data.Where(x =>
                    x.DepartmentName.Contains(query.DepartmentId));
            }

            // status
            if (!string.IsNullOrEmpty(query.Status))
            {
                data = data.Where(x =>
                    x.Employee.Status == query.Status);
            }

            var total = data.Count();

            var result = data
                .OrderByDescending(x => x.Employee.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new EmployeeDto
                {
                    Id = x.Employee.Id,
                    FullName = x.Employee.FullName,
                    Department = x.DepartmentName,
                    Position = x.Employee.Position,
                    Status = x.Employee.Status
                })
                .ToList();

            return new
            {
                total,
                page = query.Page,
                pageSize = query.PageSize,
                totalPages = (int)Math.Ceiling((double)total / query.PageSize),
                data = result
            };
        }
    }
}



