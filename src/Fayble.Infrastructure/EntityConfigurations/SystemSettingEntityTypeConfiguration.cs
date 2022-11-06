using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.SystemSetting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class SystemSettingEntityTypeConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSetting");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                setting => setting.ToString(),
                setting => (SystemSettingKey) Enum.Parse(typeof(SystemSettingKey), setting));
    }
}