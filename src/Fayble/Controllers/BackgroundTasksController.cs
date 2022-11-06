using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Models.BackgroundTask;
using Fayble.Security.Authorisation;
using Fayble.Services.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackgroundTask = Fayble.Models.BackgroundTask.BackgroundTask;


namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
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
       // await _backgroundTaskService.qu(backgroundTaskRequest.ItemId, taskType);y
        return Accepted();
    }
}
