namespace Fayble.Services.BackgroundServices;

public interface IBackgroundTaskHub
{
    Task SendDescriptionUpdate(Guid id, string description);
    Task OnConnectedAsync();
}