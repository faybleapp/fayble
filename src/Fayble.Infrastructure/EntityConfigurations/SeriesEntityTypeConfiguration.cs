using Fayble.Domain.Aggregates.Series;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class SeriesEntityTypeConfiguration : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.ToTable("Series");

        builder.HasOne(e => e.ParentSeries)
            .WithMany()
            .HasForeignKey(e => e.ParentSeriesId);

        builder.HasOne(e => e.FieldLocks)
            .WithOne()
            .HasForeignKey<SeriesFieldLocks>(e => e.SeriesId);

        builder.HasMany(e => e.Books)
            .WithOne(x => x.Series)
            .HasForeignKey(x => x.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Publisher)
            .WithMany()
            .HasForeignKey(e => e.PublisherId);
    }
}