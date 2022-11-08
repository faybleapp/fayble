using Fayble.Core.Enums;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace Fayble.Core.Helpers;

public static class ApplicationHelpers
{
    public static LoggingLevelSwitch LogLevel = new();

    public static Application GetApplication()
    {
        return Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Fayble" => Application.Fayble,
            "Fayble.Service" => Application.Service,
            _ => Application.Fayble
        };
    }

    public static string GetAppDirectory()
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.environment.specialfolder?view=net-7.0
        var folder =  IsDocker()
            ? "/config" :  Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.CommonApplicationData), "Fayble");

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        return folder;
    }

    public static string GetMediaDirectoryRoot(Guid id)
    {
        var rootFolder = 0;
        var rootPath = GetMediaDirectory();
        var path = Path.Combine(rootPath, rootFolder.ToString());

        while (Directory.Exists(path) && Directory.GetDirectories(path).Count() >= 5000)
        {
            rootFolder++;
            path = Path.Combine(rootPath, rootFolder.ToString());
        }

        var pathWithId = Path.Combine(path, id.ToString());

        if (!Directory.Exists(pathWithId))
        {
            Directory.CreateDirectory(pathWithId);
        }

        return Path.Combine(rootFolder.ToString());
    }

    public static string GetMediaDirectory()
    {
        var mediaFolder = Path.Combine(GetAppDirectory(), "Media");

        if (!Directory.Exists(mediaFolder))
        {
            Directory.CreateDirectory(mediaFolder);
        }

        return mediaFolder;
    }

    public static string GetTokenSigningKey()
    {
        var signingTokenKeyFile = Path.Combine(GetConfigDirectory(), "SigningTokenKey");

        if (!File.Exists(signingTokenKeyFile))
        {
            File.WriteAllText(signingTokenKeyFile, Guid.NewGuid().ToString());
        }

        return File.ReadAllText(signingTokenKeyFile);
    }

    public static string GetConfigDirectory()
    {
        var configFolder = Path.Combine(GetAppDirectory(), "Config");

        if (!Directory.Exists(configFolder))
        {
            Directory.CreateDirectory(configFolder);
        }

        return configFolder;
    }

    public static string GetLogsDirectory()
    {
        var configFolder = Path.Combine(GetAppDirectory(), "Logs");

        if (!Directory.Exists(configFolder))
        {
            Directory.CreateDirectory(configFolder);
        }

        return configFolder;
    }

    public static bool IsDocker() => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

    public static void SetLogLevel(LogEventLevel logLevel)
    {
        LogLevel.MinimumLevel = LogEventLevel.Debug;
    }
}