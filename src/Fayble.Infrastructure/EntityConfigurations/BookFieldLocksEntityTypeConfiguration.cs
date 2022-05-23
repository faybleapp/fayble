using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookFieldLocksEntityTypeConfiguration : IEntityTypeConfiguration<BookFieldLocks>
{
    public void Configure(EntityTypeBuilder<BookFieldLocks> builder)
    {
        builder.ToTable("BookFieldLocks");

        builder.HasKey(e => e.BookId);
    }
}