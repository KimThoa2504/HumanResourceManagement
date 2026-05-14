using HumanResourceManagement.Services.Dashboards;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceManagement.Controllers.Dashboards
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController: ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            return Ok(_service.GetDashboard());
        }
    }
}
