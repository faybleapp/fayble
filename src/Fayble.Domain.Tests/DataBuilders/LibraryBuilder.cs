using System;
using System.Collections.Generic;
using System.Linq;
using Fayble.Domain.Aggregates.Library;

namespace Fayble.Domain.Tests.DataBuilders;

public class LibraryBuilder : TestDataBuilder<Library>
{
    public static LibraryBuilder WithDefaults()
    {
        var builder = new LibraryBuilder()
            .WithId(Guid.NewGuid())
            .WithName("Test Library")
            .WithType(LibraryType.ComicBook)
            .WithPaths(
                new[]
                {
                    new LibraryPath(string.Empty, Guid.NewGuid())
                })
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

    public LibraryBuilder WithType(LibraryType type)
    {
        Set(i => i.Type, type);
        return this;
    }

    public LibraryBuilder WithPaths(IReadOnlyCollection<LibraryPath> paths)
    {
        Set(i => i.Paths, paths);
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
            Get(i => i.Paths).Select(p => p.Path),
            Get(i => i.Settings));

        return library;
    }
}
