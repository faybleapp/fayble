using System.Reflection;
using System.Runtime.InteropServices;
using Fayble.Core.Enums;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Fayble.Core.Helpers;

public static class ApplicationHelpers
{
    public static LoggingLevelSwitch LogLevel = new();
    public static string? AppDirectoryOverride = null;

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
        var folder = AppDirectoryOverride ?? Environment.GetFolderPath(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Environment.SpecialFolder.CommonApplicationData
                : Environment.SpecialFolder.UserProfile);

        var fullPath = Path.Combine(folder!, "Fayble");

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        return fullPath;
    }

    public static string GetMediaDirectory(string type, Guid id)
    {
        var folder = 0;
        var rootPath = Path.Combine(GetAppDirectory(), "Media", type);
        var path = Path.Combine(rootPath, folder.ToString());

        while (Directory.Exists(path) && Directory.GetDirectories(path).Count() >= 5000)
        {
            folder++;
            path = Path.Combine(rootPath, folder.ToString());
        }

        var pathWithId = Path.Combine(path, id.ToString());
        if (!Directory.Exists(pathWithId)) Directory.CreateDirectory(pathWithId);

        return Path.Combine("Media", type, folder.ToString(), id.ToString());
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

    public static void SetLogLevel(LogEventLevel logLevel)
    {
        LogLevel.MinimumLevel = LogEventLevel.Debug;
    }
}