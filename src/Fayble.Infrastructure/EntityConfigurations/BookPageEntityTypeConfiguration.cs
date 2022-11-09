using Fayble.Domain.Aggregates.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookPageEntityTypeConfiguration : IEntityTypeConfiguration<BookPage>
{
    public void Configure(EntityTypeBuilder<BookPage> builder)
    {
        builder.ToTable("BookPage");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}