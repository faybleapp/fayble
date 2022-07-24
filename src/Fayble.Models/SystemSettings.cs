namespace Fayble.Models;

public class SystemSettings
{
    public bool FirstRun { get; }
       
    public SystemSettings(bool firstRun)
    {
        FirstRun = firstRun;
    }
}