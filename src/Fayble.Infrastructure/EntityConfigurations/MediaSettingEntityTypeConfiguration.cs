using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.MediaSetting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class MediaSettingEntityTypeConfiguration : IEntityTypeConfiguration<MediaSetting>
{
    public void Configure(EntityTypeBuilder<MediaSetting> builder)
    {
        builder.ToTable("MediaSetting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                setting => setting.ToString(),
                setting => (MediaSettingKey) Enum.Parse(typeof(MediaSettingKey), setting));
    }
}