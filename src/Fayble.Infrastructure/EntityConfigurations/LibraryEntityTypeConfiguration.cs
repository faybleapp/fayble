﻿using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class LibraryEntityTypeConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.ToTable("Library");

        builder.HasMany(e => e.Series)
            .WithOne(s => s.Library)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Settings)
            .WithOne(s => s.Library)
            .HasForeignKey(s => s.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Books)
            .WithOne(b => b.Library)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Type)
            .HasConversion(
                applicationType => applicationType.ToString(),
                applicationType => (MediaType) Enum.Parse(typeof(MediaType), applicationType));
    }
}