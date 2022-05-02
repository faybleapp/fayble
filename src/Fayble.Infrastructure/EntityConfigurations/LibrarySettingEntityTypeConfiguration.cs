using Fayble.Domain.Aggregates.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class LibrarySettingEntityTypeConfiguration : IEntityTypeConfiguration<LibrarySetting>
{
    public void Configure(EntityTypeBuilder<LibrarySetting> builder)
    {
        builder.ToTable("LibrarySetting");
        builder.HasKey(e => new {Setting = e.Setting, e.LibraryId});
        builder.Property(x => x.Setting)
            .HasConversion(
                setting => setting.ToString(),
                setting => (LibrarySettingKey) Enum.Parse(typeof(LibrarySettingKey), setting));
    }
}