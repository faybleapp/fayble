using Fayble.Models;
using Fayble.Services.System;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fayble.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;

        public SystemController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        [HttpPost("first-run")]
        public async Task Post([FromBody] FirstRun firstRun)
        {
            await _systemService.FirstRun(firstRun);
        }

        [HttpGet("settings")]
        public async Task<SystemSettings> GetConfiguration()
        {
            return await _systemService.GetConfiguration();
        }
    }
}
