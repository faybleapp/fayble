﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Tests.DataBuilders;

public class LibraryBuilder : TestDataBuilder<Library>
{
    public static LibraryBuilder WithDefaults()
    {
        var builder = new LibraryBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Test Library")
            .WithType(MediaType.ComicBook)
            .WithFolderPath("\\")
            .WithSettings(new []
            {
                new LibrarySetting(LibrarySettingKey.ReviewOnImport, true.ToString())
            });

        return builder;
    }


    public LibraryBuilder WithId(Guid id)
    {
        Set(i => i.Id, id);
        return this;
    }

    public LibraryBuilder WithName(string name)
    {
        Set(i => i.Name, name);
        return this;
    }

    public LibraryBuilder WithType(MediaType type)
    {
        Set(i => i.Type, type);
        return this;
    }

    public LibraryBuilder WithFolderPath(string folderPath)
    {
        Set(i => i.FolderPath, folderPath);
        return this;
    }

    public LibraryBuilder WithSettings(IReadOnlyCollection<LibrarySetting> settings)
    {
        Set(i => i.Settings, settings);
        return this;
    }

    public override Library Build()
    {
        var library = new Library(
            Get(i => i.Id),
            Get(i => i.Name),
            Get(i => i.Type),
            Get(i => i.FolderPath),
            Get(i => i.Settings));

        return library;
    }
}
