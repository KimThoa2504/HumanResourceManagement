using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.DTOs.LeaveRequests;
using HumanResourceManagement.Models.LeaveRequests;

namespace HumanResourceManagement.Services.LeaveRequests
{
    public class LeaveRequestService
    {
        private readonly AppDbContext _context;

        public LeaveRequestService(AppDbContext context)
        {
            _context = context;
        }
        //create leave request
        public async Task<LeaveRequestDto> Create(CreateLeaveRequestDto dto)
        {
            if (dto.EndDate < dto.StartDate)
                throw new ApiException("End date must be greater than start date");

            var employee = _context.Employees
                .FirstOrDefault(x => x.Id == dto.EmployeeId);

            if (employee == null)
                throw new ApiException("Employee not found");

            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason
            };

            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();

            return new LeaveRequestDto
            {
                Id = leave.Id,
                EmployeeName = employee.FullName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                Status = leave.Status
            };
        }

        //get leave request
        public List<LeaveRequestDto> GetAll()
        {
            var employees = _context.Employees.ToList();

            return _context.LeaveRequests
                .ToList()
                .Select(x => new LeaveRequestDto
                {
                    Id = x.Id,
                    EmployeeName = employees
                        .FirstOrDefault(e => e.Id == x.EmployeeId)?.FullName ?? "",
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Reason = x.Reason,
                    Status = x.Status
                }).ToList();
        }

        //Approve or Reject
        public async Task UpdateStatus(int id, UpdateLeaveStatusDto dto)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null)
                throw new ApiException("Leave request not found");

            leave.Status = dto.Status;

            await _context.SaveChangesAsync();
        }

        public object Search(LeaveRequestQuery query)
        {
            var data = _context.LeaveRequests.AsQueryable();

            // employee
            if (query.EmployeeId.HasValue)
            {
                data = data.Where(x =>
                    x.EmployeeId == query.EmployeeId);
            }

            // status
            if (!string.IsNullOrEmpty(query.Status))
            {
                data = data.Where(x =>
                    x.Status == query.Status);
            }

            // date
            if (query.StartDate.HasValue)
            {
                data = data.Where(x =>
                    x.StartDate.Date == query.StartDate.Value.Date);
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
                    x.StartDate,
                    x.EndDate,
                    x.Reason,
                    x.Status,

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
