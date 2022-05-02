namespace Fayble.Models;

public class SystemConfiguration
{
    public bool FirstRun { get; private set; }
    
    public SystemConfiguration(bool firstRun)
    {
        FirstRun = firstRun;
    }
}