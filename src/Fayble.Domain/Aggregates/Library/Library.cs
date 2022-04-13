using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.Library;

public class Library : AuditableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public MediaType Type { get; private init; }
    public virtual IEnumerable<Series.Series> Series { get; private set; }
    public virtual IReadOnlyCollection<Book.Book> Books { get; private set; }

    private readonly List<LibraryPath> _paths = new();
    public virtual IReadOnlyCollection<LibraryPath> Paths => _paths;

    private readonly List<LibrarySetting> _settings = new();
    public virtual IReadOnlyCollection<LibrarySetting> Settings => _settings;

    public Library()
    {
    }

    public Library(
        Guid id,
        string name,
        MediaType type,
        IEnumerable<string> paths,
        IEnumerable<LibrarySetting> settings) : base(id)
    {
        Guard.AgainstEmpty(id, nameof(Id));
        Guard.AgainstNullOrWhitespace(name, nameof(Name));

        Name = name;
        Type = type;
        _settings = settings.ToList();

        foreach (var path in paths) _paths.Add(new LibraryPath(Guid.NewGuid(), path));
    }

    public void Update(string name, IEnumerable<string> paths)
    {
        Name = name;
        var newPaths = paths.ToArray();
        var existingPaths = _paths.Select(p => p.Path).ToArray();

        _paths.RemoveAll(p => existingPaths.Except(newPaths, StringComparer.OrdinalIgnoreCase).Any(ptr =>
                string.Equals(ptr, p.Path, StringComparison.CurrentCultureIgnoreCase)));

        _paths.AddRange(newPaths.Except(existingPaths, StringComparer.OrdinalIgnoreCase)
            .Select(path => new LibraryPath(Guid.NewGuid(), path)));
    }

    public void UpdateSetting(LibrarySettingKey setting, string value)
    {
        var librarySetting = _settings.First(s => s.Setting == setting);
        librarySetting.Update(value);
    }

    public string GetSetting(LibrarySettingKey setting)
    {
        return _settings.First(s => s.Setting == setting).Value;
    }
}