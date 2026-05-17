using HumanResourceManagement.Models.DTOs.Employees;
using HumanResourceManagement.Services.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Employees
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,HR")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeesController(EmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Task.Run(() => _service.GetAll())); 
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok("Delete");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
        {
            return Ok(await _service.Update(id, dto));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] EmployeeQuery query)
        {
            return Ok(_service.Search(query));
        }

    }
}
