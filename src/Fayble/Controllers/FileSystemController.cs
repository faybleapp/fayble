using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FileSystemController : ControllerBase
{
    [HttpGet("pathexists")]
    public bool PathExists(string path)
    {
        return Directory.Exists(path);
    }
}
