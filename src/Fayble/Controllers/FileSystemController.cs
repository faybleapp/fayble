using Fayble.Models;
using Fayble.Models.FileSystem;
using Fayble.Models.Import;
using Fayble.Security.Authorisation;
using Fayble.Services.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class FileSystemController : ControllerBase
{
    private readonly IComicBookFileSystemService _comicBookFileSystemService;

    public FileSystemController(IComicBookFileSystemService comicBookFileSystemService)
    {
        _comicBookFileSystemService = comicBookFileSystemService;
    }

    [HttpPost("pathexists")]
    public bool PathExists(PathValidation pathValidation)
    {
        return Directory.Exists(pathValidation.Path);
    }

    [HttpPost("generate-filename")]
    public async Task<ActionResult<string>> GenerateFilename([FromBody] GenerateFilenameRequest request)
    {
        return await _comicBookFileSystemService.GenerateFilename(request);
    }

    [HttpPost("file-exists")]
    public async Task<ActionResult<bool>> FileExists([FromBody] FileExistsRequest request)
    {
        return await _comicBookFileSystemService.FileExists(request);
    }
}