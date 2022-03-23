using Fayble.BackgroundServices;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Models.BackgroundTask;
using Microsoft.AspNetCore.Mvc;
using BackgroundTask = Fayble.BackgroundServices.BackgroundTask;


namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackgroundTasksController : ControllerBase
{
    private readonly IBackgroundTaskService _backgroundTaskService;

    public BackgroundTasksController(IBackgroundTaskService backgroundTaskService)
    {
        _backgroundTaskService = backgroundTaskService;
    }

    [HttpGet]
    public async Task<IEnumerable<BackgroundTask>> GetAll()
    {
        return new List<BackgroundTask>();
    }

    [HttpPost]
    public async Task<IActionResult> Run(BackgroundTaskRequest backgroundTaskRequest)
    {
        var taskType = Enum.Parse<BackgroundTaskType>(backgroundTaskRequest.TaskType);
        await _backgroundTaskService.Run(backgroundTaskRequest.ItemId, taskType);
        return Accepted();
    }
}
