using Fayble.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : Controller
{
    [HttpGet("{mediaPath}")]
    public ActionResult GetImage(string mediaPath)
    {
        return PhysicalFile(Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath), "image/jpeg");
    }
}