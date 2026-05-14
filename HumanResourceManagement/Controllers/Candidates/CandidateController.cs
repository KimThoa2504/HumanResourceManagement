using HumanResourceManagement.Models.DTOs.Candidates;
using HumanResourceManagement.Services.Candidates;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Candidates
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly CandidateService _service;
        public CandidateController(CandidateService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCandidateDto dto)
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
       UpdateCandidateStatusDto dto)
        {
            await _service.UpdateStatus(id, dto);

            return Ok("Updated");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] CandidateQuery query)
        {
            return Ok(_service.Search(query));
        }
    }
}
