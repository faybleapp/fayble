

#nullable disable warnings
#nullable disable annotations

using System.Reflection;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Publisher;
using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Entities;
using Fayble.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User = Fayble.Domain.Aggregates.User.User;

namespace Fayble.Infrastructure;

public interface IFaybleDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

public class FaybleDbContext : IdentityDbContext<User, UserRole, Guid>, IFaybleDbContext
{
    public DbSet<Library> Libraries { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<SystemConfiguration> SystemConfiguration { get; set; }
    public DbSet<BackgroundTask> BackgroundTasks { get; set; }
    public DbSet<BookTag> BookTags { get; set; }

    private readonly IUser _userIdentity;

    public FaybleDbContext(DbContextOptions options, IUser userIdentity) : base(options)
    {
        _userIdentity = userIdentity;
    }

    public FaybleDbContext()
    {
    } // Required for EF
    
    public override int SaveChanges()
    {
        SetMetadata();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetMetadata();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetMetadata()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(
                e => e.Entity is IAuditable && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));


        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is IAuditable lastModified)
                lastModified.SetLastModified(_userIdentity.Id, DateTime.UtcNow);

            if (entityEntry.State != EntityState.Added) continue;

            if (entityEntry.Entity is IAuditable entity) entity.SetCreated(_userIdentity.Id, DateTime.UtcNow);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Override ASP.Net default table names, e.g. AspNetRoles
        modelBuilder.Entity<User>(entity => entity.ToTable("User"));
        modelBuilder.Entity<UserRole>(entity => entity.ToTable("Role"));
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("UserRoleClaim");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");

        //Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}