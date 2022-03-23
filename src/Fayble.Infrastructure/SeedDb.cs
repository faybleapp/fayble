using Fayble.Core.Helpers;
using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Publisher;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fayble.Infrastructure;

public static class SeedDb
{
    public static void MigrateAndSeedDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<FaybleDbContext>();
        context?.Migrate();
        context?.Seed();
    }

    private static void Migrate(this FaybleDbContext context)
    {
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }

    private static void Seed(this FaybleDbContext context)
    {
        context.AddOrUpdateSeedData();
        context.SaveChanges();
    }

    private static void AddOrUpdateSeedData(this FaybleDbContext context)
    {
        if (!context.FileTypes.Any())
        {
            context.FileTypes.AddRange(new List<FileType>
                {
                    new("cbr", LibraryType.ComicBook),
                    new("cbz", LibraryType.ComicBook),
                    new("epub", LibraryType.Book),
                    new("mobi", LibraryType.Book)
                });
        }

        if (!context.Configuration.Any())
        {
            context.Configuration.AddRange(new List<Configuration>
                {
                    new(Setting.ReviewOnImport, "true")
                });

        }

        if (!context.Publishers.Any())
        {
            var publishers = new List<Publisher>
                {
                    new Publisher("Marvel"),
                    new Publisher("DC Comics"),
                    new Publisher("Image Comics"),
                    new Publisher("IDW Publishing")
                };

            publishers.ForEach(p => p.SetMediaPath(ApplicationHelpers.GetMediaDirectory(p.GetType().Name, p.Id)));

            context.Publishers.AddRange(publishers);
        }
    }
}