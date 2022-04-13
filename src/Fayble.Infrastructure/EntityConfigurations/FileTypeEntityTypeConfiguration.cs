using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class FileTypeConfiguration : IEntityTypeConfiguration<FileType>
{
    public void Configure(EntityTypeBuilder<FileType> builder)
    {
        builder.ToTable("FileType");
        builder.Property(x => x.MediaType)
            .HasConversion(
                applicationType => applicationType.ToString(),
                applicationType => (MediaType)Enum.Parse(typeof(MediaType), applicationType));
    }
}
 