using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class FileTypeConfiguration : IEntityTypeConfiguration<FileType>
{
    public void Configure(EntityTypeBuilder<FileType> builder)
    {
        builder.ToTable("FileType");
        builder.Property(x => x.LibraryType)
            .HasConversion(
                applicationType => applicationType.ToString(),
                applicationType => (LibraryType)Enum.Parse(typeof(LibraryType), applicationType));
    }
}
 