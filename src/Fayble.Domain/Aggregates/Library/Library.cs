using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.Library;

public class Library : AuditableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public MediaType Type { get; private init; }
    public string FolderPath { get; set; }

    public virtual IEnumerable<Series.Series> Series { get; private set; }
    public virtual IReadOnlyCollection<Book.Book> Books { get; private set; }

    private readonly List<LibrarySetting> _settings = new();
    public virtual IReadOnlyCollection<LibrarySetting> Settings => _settings;

    public Library()
    {
    }

    public Library(
        Guid id,
        string name,
        MediaType type,
        string folderPath,
        IEnumerable<LibrarySetting> settings) : base(id)
    {
        Guard.AgainstEmpty(id, nameof(Id));
        Guard.AgainstNullOrWhitespace(name, nameof(Name));

        Name = name;
        Type = type;
        FolderPath = folderPath;
        _settings = settings.ToList();
    }

    public void Update(string name, string folderPath)
    {
        Name = name;
        FolderPath = folderPath;
    }

    public void UpdateSetting(LibrarySettingKey setting, string value)
    {
        var librarySetting = _settings.First(s => s.Setting == setting);
        librarySetting.Update(value);
    }

    public T GetSetting<T>(LibrarySettingKey setting)
    {
        return (T)Convert.ChangeType(_settings.First(s => s.Setting == setting).Value, typeof(T));
    }
}