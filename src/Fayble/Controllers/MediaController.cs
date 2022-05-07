using Fayble.Core.Helpers;
using Fayble.Security.Authorisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class MediaController : Controller
{
    [HttpGet("{mediaPath}")]
    [AllowAnonymous]
    public ActionResult GetImage(string mediaPath)
    {
        return PhysicalFile(Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath), "image/jpeg");
    }
}