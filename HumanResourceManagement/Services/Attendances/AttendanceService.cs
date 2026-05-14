using HumanResourceManagement.Data;
using HumanResourceManagement.Exceptions;
using HumanResourceManagement.Models.Attendances;
using HumanResourceManagement.Models.DTOs.Attendances;

namespace HumanResourceManagement.Services.Attendances
{
    public class AttendanceService
    {
        private readonly AppDbContext _context;

        public AttendanceService(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<AttendanceDto> CheckIn(CheckInDto dto)
        {
            var today = DateTime.Today;

            var existed = _context.Attendances
                .FirstOrDefault(x => x.EmployeeId == dto.EmployeeId && x.WorkDate == today);

            if (existed != null)
                throw new ApiException("Already checked in today");

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                WorkDate = today,
                CheckIn = DateTime.Now.TimeOfDay
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                Id = attendance.Id,
                EmployeeId = attendance.EmployeeId,
                WorkDate = attendance.WorkDate,
                CheckIn = attendance.CheckIn
            };
        }

        public async Task<AttendanceDto> CheckOut(CheckOutDto dto)
        {
            var today = DateTime.Today;

            var attendance = _context.Attendances
                .FirstOrDefault(x => x.EmployeeId == dto.EmployeeId && x.WorkDate == today);

            if (attendance == null)
                throw new ApiException("Not checked in yet");

            if (attendance.CheckOut != null)
                throw new ApiException("Already checked out");

            attendance.CheckOut = DateTime.Now.TimeOfDay;

            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                Id = attendance.Id,
                EmployeeId = attendance.EmployeeId,
                WorkDate = attendance.WorkDate,
                CheckIn = attendance.CheckIn,
                CheckOut = attendance.CheckOut
            };
        }

        // get attendance by employee
        public List<AttendanceDto> GetByEmployee(int employeeId)
        {
            return _context.Attendances
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.WorkDate)
                .Select(x => new AttendanceDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    WorkDate = x.WorkDate,
                    CheckIn = x.CheckIn,
                    CheckOut = x.CheckOut
                }).ToList();
        }

        public object Search(AttendanceQuery query)
        {
            var data = _context.Attendances.AsQueryable();

            if (query.EmployeeId.HasValue)
            {
                data = data.Where(x =>
                    x.EmployeeId == query.EmployeeId);
            }

            if (query.WorkDate.HasValue)
            {
                data = data.Where(x =>
                    x.WorkDate.Date == query.WorkDate.Value.Date);
            }

            var total = data.Count();

            var result = data
                .OrderByDescending(x => x.WorkDate)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new
            {
                total,
                data = result
            };
        }
    }
}
