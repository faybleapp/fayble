using Fayble.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    [HttpGet("logs")]
    public ActionResult GetLogs()
    {
        return PhysicalFile($"{ApplicationHelpers.GetLogsDirectory()}/fayble.log", "text/plain");  
    }
}