using Fayble.Models;
using Fayble.Security.Authorisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class FileSystemController : ControllerBase
{
    [HttpPost("pathexists")]
    public bool PathExists(PathValidation pathValidation)
    {
        return Directory.Exists(pathValidation.Path);
    }
}
