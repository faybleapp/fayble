using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookPersonEntityTypeConfiguration : IEntityTypeConfiguration<BookPerson>
{
    public void Configure(EntityTypeBuilder<BookPerson> builder)
    {
        builder.ToTable("BookPerson");

        builder.HasKey(e => new {e.BookId, e.PersonId, e.Role});

        builder.HasOne(e => e.Book)
            .WithMany(b => b.People)
            .HasForeignKey(e => e.BookId);
        
        builder.HasOne(e => e.Person)
            .WithMany(b => b.Books)
            .HasForeignKey(e => e.PersonId);
        
        builder.Property(e => e.Role)
            .HasConversion(
                role => role.ToString(),
                role => (RoleType) Enum.Parse(typeof(RoleType), role));
    }
}