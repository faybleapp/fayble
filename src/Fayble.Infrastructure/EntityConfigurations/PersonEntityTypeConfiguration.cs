using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.People;
using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Person");

        builder.HasMany(e => e.Books)
            .WithMany(p => p.People);

        builder.Property(x => x.PersonType)
            .HasConversion(
                personType => personType.ToString(),
                personType => (PersonType) Enum.Parse(typeof(PersonType), personType));
    }
}