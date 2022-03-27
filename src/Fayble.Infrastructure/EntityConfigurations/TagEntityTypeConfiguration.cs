using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class TagPathEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tag");
        builder.HasKey(x => x.Id);
    }
}