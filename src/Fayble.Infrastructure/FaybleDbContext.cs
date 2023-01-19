

#nullable disable warnings
#nullable disable annotations

using System.Reflection;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Domain.Aggregates.Publisher;
using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.SystemSetting;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Entities;
using Fayble.Security.Models;
using MediatR;
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
    public DbSet<SystemSetting> SystemConfiguration { get; set; }
    public DbSet<BackgroundTask> BackgroundTasks { get; set; }
    public DbSet<BookTag> BookTags { get; set; }
    public DbSet<MediaSetting> MediaSettings { get; set; }

    private readonly IUser _userIdentity;
    private readonly IMediator _mediator;

    public FaybleDbContext(DbContextOptions options, IUser userIdentity, IMediator mediator) : base(options)
    {
        _userIdentity = userIdentity;
        _mediator = mediator;
    }

    public FaybleDbContext()
    {
    } // Required for EF
    
    public override int SaveChanges()
    {
        SetMetadata();
        return SaveChangesAsync().Result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetMetadata();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_mediator == null) return result;

        var domainEntities = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        var domainEvents = domainEntities
            .SelectMany(e => e?.DomainEvents)
            .ToArray();

        domainEntities.ToList().ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    private void SetMetadata()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(
                e => e.Entity is IAuditableEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));


        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is IAuditableEntity lastModified)
                lastModified.SetLastModified(_userIdentity.Id, DateTime.UtcNow);

            if (entityEntry.State != EntityState.Added) continue;

            if (entityEntry.Entity is IAuditableEntity entity) entity.SetCreated(_userIdentity.Id, DateTime.UtcNow);
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