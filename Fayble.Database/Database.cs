using System.Reflection;
using DbUp;
using Fayble.Core.Helpers;
using Serilog;

namespace Fayble.Database;

public static class Database
{
    public static void Migrate()
    {
        var connectionString = $"Data Source={Path.Combine(ApplicationHelpers.GetAppDirectory(), "Fayble.db")}";
        
        var upgrader =
            DeployChanges.To
                .SQLiteDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogScriptOutput()
                .LogToAutodetectedLog()
                .LogToConsole()
                .Build();
        
        var result = upgrader.PerformUpgrade();
        
        if (!result.Successful)
        {
            Log.Logger.Error(result.Error, "An error occurred while migrating database");
        }
    }
}