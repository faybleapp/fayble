using Fayble.Models.Import;
using Fayble.Services.Import;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportController(IImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("scan")]
    public async Task<IEnumerable<ImportScanFile>> Scan(ImportScanRequest import)
    {
        return await _importService.Scan(import.Path);
    }

    [HttpPost]
    public async Task Import(List<ImportFileRequest> importFiles)
    {
        await _importService.Import(importFiles);
    }
}

