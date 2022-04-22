using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class UserSettingEntityTypeConfiguration : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.ToTable("UserSetting");
        builder.HasKey(e => new {Setting = e.Setting, e.UserId});
        builder.Property(x => x.Setting)
            .HasConversion(
                setting => setting.ToString(),
                setting => (UserSettingKey) Enum.Parse(typeof(UserSettingKey), setting));
    }
}