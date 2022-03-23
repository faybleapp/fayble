namespace Fayble.Models.Library;

public class Library
{
    public Guid? Id { get; }

    public string Name { get; }

    public string LibraryType { get; }

    public List<string>? Paths { get; }

    public LibrarySettings Settings { get; }

    public Library(Guid? id, string name, string libraryType, List<string>? paths, LibrarySettings settings)
    {
        Id = id;
        Name = name;
        LibraryType = libraryType;
        Paths = paths;
        Settings = settings;
    }
}
