using Fayble.Core.Helpers;
using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Publisher;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Enums;
using Fayble.Security.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
                    new("cbr", MediaType.ComicBook),
                    new("cbz", MediaType.ComicBook),
                    new("epub", MediaType.Book),
                    new("mobi", MediaType.Book)
                });
        }

        if (!context.Configuration.Any())
        {
            context.Configuration.AddRange(new List<Configuration>
                {
                    new(ConfigurationKey.ReviewOnImport, "true")
                });

        }

        if (!context.Publishers.Any())
        {
            var publishers = new List<Publisher>
                {
                    new ("Marvel"),
                    new ("DC Comics"),
                    new ("Image Comics"),
                    new ("IDW Publishing")
                };

            publishers.ForEach(p => p.SetMediaPath(ApplicationHelpers.GetMediaDirectory(p.GetType().Name, p.Id)));

            context.Publishers.AddRange(publishers);
        }

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.Owner
                },
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.Administrator
                },
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.User
                });
        }
    }
}