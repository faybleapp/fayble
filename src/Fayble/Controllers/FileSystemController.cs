using Fayble.Security.Authorisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class FileSystemController : ControllerBase
{
    [HttpGet("pathexists")]
    public bool PathExists(string path)
    {
        return Directory.Exists(path);
    }
}
