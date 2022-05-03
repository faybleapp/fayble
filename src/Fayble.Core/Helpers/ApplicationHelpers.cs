using System.Reflection;
using System.Runtime.InteropServices;
using Fayble.Core.Enums;

namespace Fayble.Core.Helpers;

public static class ApplicationHelpers
{
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
        var folder = Environment.GetEnvironmentVariable(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "ProgramData"
                : "Home");

        return Path.Combine(folder!, "Fayble");
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

    private static string GetConfigDirectory()
    {
        var configFolder = Path.Combine(GetAppDirectory(), "Config");

        if (!Directory.Exists(configFolder))
        {
            Directory.CreateDirectory(configFolder);
        }

        return configFolder;
    }
}