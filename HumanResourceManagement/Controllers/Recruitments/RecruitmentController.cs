using HumanResourceManagement.Models.DTOs.Recruitments;
using HumanResourceManagement.Services.Recruitments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Recruitments
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,HR")]
    public class RecruitmentController : ControllerBase
    {
        private readonly RecruitmentService _service;

        public RecruitmentController(RecruitmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRecruitmentDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            await _service.Close(id);

            return Ok("Recruitment closed");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] RecruitmentQuery query)
        {
            return Ok(_service.Search(query));
        }
    }
}
