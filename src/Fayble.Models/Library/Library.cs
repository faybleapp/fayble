namespace Fayble.Models.Library;

public class Library
{
    public Guid? Id { get; }

    public string Name { get; }

    public string LibraryType { get; }

    public string FolderPath { get; }

    public LibrarySettings Settings { get; }

    public Library(Guid? id, string name, string libraryType, string folderPath, LibrarySettings settings)
    {
        Id = id;
        Name = name;
        LibraryType = libraryType;
        FolderPath = folderPath;
        Settings = settings;
    }
}
