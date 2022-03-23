using Fayble.Domain.Aggregates.Format;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class FormatConfiguration : IEntityTypeConfiguration<Format>
{
    public void Configure(EntityTypeBuilder<Format> builder)
    {
        builder.ToTable("Format");
        builder.Property(x => x.MediaType)
            .HasConversion(
                mediaType => mediaType.ToString(),
                mediaType => (MediaType) Enum.Parse(typeof(MediaType), mediaType));
    }
}