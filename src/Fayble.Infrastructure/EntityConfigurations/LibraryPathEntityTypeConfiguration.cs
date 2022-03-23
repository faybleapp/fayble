using Fayble.Domain.Aggregates.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class LibraryPathEntityTypeConfiguration : IEntityTypeConfiguration<LibraryPath>
{
    public void Configure(EntityTypeBuilder<LibraryPath> builder)
    {
        builder.ToTable("LibraryPath");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Library)
            .WithMany(x => x.Paths)
            .HasForeignKey(x => x.LibraryId);
    }
}