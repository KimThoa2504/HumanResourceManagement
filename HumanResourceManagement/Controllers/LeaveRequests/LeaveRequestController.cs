using HumanResourceManagement.Models.DTOs.LeaveRequests;
using HumanResourceManagement.Services.LeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.LeaveRequests
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,HR")]
    public class LeaveRequestController: ControllerBase
    {
        private readonly LeaveRequestService _service;

        public LeaveRequestController(LeaveRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
       int id,
       UpdateLeaveStatusDto dto)
        {
            await _service.UpdateStatus(id, dto);

            return Ok("Updated");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] LeaveRequestQuery query)
        {
            return Ok(_service.Search(query));
        }
    }
}
