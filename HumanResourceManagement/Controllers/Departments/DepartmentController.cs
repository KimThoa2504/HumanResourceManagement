using HumanResourceManagement.Models.DTOs.Departments;
using HumanResourceManagement.Services.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Departments
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,HR")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _service;
        public DepartmentController(DepartmentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("Delete success");
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] DepartmentQuery query)
        {
            return Ok(_service.Search(query));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDto dto
        )
        {
            return Ok(await _service.Update(id, dto));
        }
    }
}
