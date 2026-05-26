using HumanResourceManagement.Models.DTOs.Users;
using HumanResourceManagement.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserDto dto)
        {
            var result = await _service.CreateUser(dto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }


        [HttpPut("{id}/lock")]
        public async Task<IActionResult> Lock(int id)
        {
            await _service.LockUser(id);
            return Ok("User locked");
        }

        [HttpPut("{id}/unlock")]
        public async Task<IActionResult> UnlockUser(int id)
        {
            await _service.UnlockUser(id);
            return Ok(new { message = "Mở khóa tài khoản thành công" });
        }
    }
}
