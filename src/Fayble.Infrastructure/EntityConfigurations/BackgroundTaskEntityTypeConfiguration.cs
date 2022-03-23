using Fayble.Domain.Aggregates.BackgroundTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BackgroundTaskTypeConfiguration : IEntityTypeConfiguration<BackgroundTask>
{
    public void Configure(EntityTypeBuilder<BackgroundTask> builder)
    {
        builder.ToTable("BackgroundTask");

        builder.Property(x => x.Type)
            .HasConversion(
                type => type.ToString(),
                type => (BackgroundTaskType)Enum.Parse(typeof(BackgroundTaskType), type));

        builder.Property(x => x.Status)
            .HasConversion(
                status => status.ToString(),
                status => (BackgroundTaskStatus) Enum.Parse(typeof(BackgroundTaskStatus), status));

    }
}