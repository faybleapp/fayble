using Fayble.Core.Helpers;
using Fayble.Security.Authorisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class MediaController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetImage(Guid id, string mediaRoot, string filename)
    {
        var filePath = Path.Combine(ApplicationHelpers.GetMediaDirectory(), mediaRoot, id.ToString(), filename);
        new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var contentType);
        var file = System.IO.File.OpenRead(filePath);

        return File(file, contentType ?? "image/jpeg");
    }
}