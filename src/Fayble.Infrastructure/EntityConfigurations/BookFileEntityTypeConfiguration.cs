﻿using Fayble.Domain.Aggregates.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fayble.Infrastructure.EntityConfigurations;

public class BookFileEntityTypeConfiguration : IEntityTypeConfiguration<BookFile>
{
    public void Configure(EntityTypeBuilder<BookFile> builder)
    {
        builder.ToTable("BookFile");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}