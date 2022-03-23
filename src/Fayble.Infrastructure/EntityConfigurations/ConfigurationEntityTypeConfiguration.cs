using Fayble.Domain.Aggregates.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.ToTable("Configuration");
        builder.Property(x => x.Id)
            .HasConversion(
                setting => setting.ToString(),
                setting => (Setting) Enum.Parse(typeof(Setting), setting));
    }
}