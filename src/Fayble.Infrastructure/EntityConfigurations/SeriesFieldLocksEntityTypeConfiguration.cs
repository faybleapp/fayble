using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class SeriesFieldLocksEntityTypeConfiguration : IEntityTypeConfiguration<SeriesFieldLocks>
{
    public void Configure(EntityTypeBuilder<SeriesFieldLocks> builder)
    {
        builder.ToTable("SeriesFieldLocks");

        builder.HasKey(e => e.SeriesId);
    }
}