using HumanResourceManagement.Models.DTOs.Attendances;
using HumanResourceManagement.Models.DTOs.Departments;
using HumanResourceManagement.Services.Attendances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Attendances
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,HR,MANAGER")]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _service;

        public AttendanceController(AttendanceService service)
        {
            _service = service;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(CheckInDto dto)
        {
            return Ok(await _service.CheckIn(dto));
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(CheckOutDto dto)
        {
            return Ok(await _service.CheckOut(dto));
        }

        [HttpGet("employee/{id}")]
        public IActionResult GetByEmployee(int id)
        {
            return Ok(_service.GetByEmployee(id));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] AttendanceQuery query)
        {
            return Ok(_service.Search(query));
        }
    }
}
