using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book");
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasOne(x => x.Series)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.SeriesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.FieldLocks)
            .WithOne()
            .HasForeignKey<BookFieldLocks>(e => e.BookId);

        builder.HasMany(e => e.ReadHistory)
            .WithOne()
            .HasForeignKey(x => x.BookId);
        
        builder.HasMany(e => e.Tags)
            .WithMany(e => e.Books);

        builder.HasOne(e => e.File)
            .WithOne(e => e.Book)
            .HasForeignKey<BookFile>(e => e.BookId);

        builder.Property(x => x.MediaType)
            .HasConversion(
                mediaType => mediaType.ToString(),
                mediaType => (MediaType) Enum.Parse(typeof(MediaType), mediaType));
    }
}