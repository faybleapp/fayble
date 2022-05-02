using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class SystemConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<SystemConfiguration>
{
    public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
    {
        builder.ToTable("SystemConfiguration");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                setting => setting.ToString(),
                setting => (SystemConfigurationKey) Enum.Parse(typeof(SystemConfigurationKey), setting));
    }
}