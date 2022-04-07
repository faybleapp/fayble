using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookTagEntityTypeConfiguration : IEntityTypeConfiguration<BookTag>
{
    public void Configure(EntityTypeBuilder<BookTag> builder)
    {
        builder.ToTable("BookTag");
        builder.HasKey(x => x.Id);
    }
}