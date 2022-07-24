using Fayble.Core.Helpers;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Publisher;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Enums;
using Fayble.Security.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fayble.Infrastructure;

public static class MigrateAndSeed
{
    public static void MigrateAndSeedDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<FaybleDbContext>();
        Database.Database.Migrate(); 
        context?.Seed();
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

        if (!context.Publishers.Any())
        {
            var publishers = new List<Publisher>
                {
                    new ("Marvel"),
                    new ("DC Comics"),
                    new ("Image Comics"),
                    new ("IDW Publishing")
                };

            publishers.ForEach(p => p.SetMediaRoot(ApplicationHelpers.GetMediaDirectoryRoot(p.Id)));

            context.Publishers.AddRange(publishers);
        }

        if (!context.SystemConfiguration.Any())
        {
            context.SystemConfiguration.AddRange(
                new List<SystemSetting>
                {
                    new(SystemSettingKey.FirstRun, true.ToString())
                });
        }

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.Owner,
                    NormalizedName = UserRoles.Owner.ToUpper()
                },
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.Administrator,
                    NormalizedName = UserRoles.Administrator.ToUpper()
                },
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = UserRoles.User,
                    NormalizedName = UserRoles.User.ToUpper()
                });
        }
    }
}