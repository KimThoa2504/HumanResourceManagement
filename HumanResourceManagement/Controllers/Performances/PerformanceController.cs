using HumanResourceManagement.Models.DTOs.Performances;
using HumanResourceManagement.Services.Performances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Performances
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,MANAGER,HR")]
    public class PerformanceController : ControllerBase
    {
        private readonly PerformanceService _service;

        public PerformanceController(PerformanceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePerformanceDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }


        [HttpGet("search")]
        public IActionResult Search([FromQuery] PerformanceQuery query)
        {
            return Ok(_service.Search(query));
        }
    }
}
