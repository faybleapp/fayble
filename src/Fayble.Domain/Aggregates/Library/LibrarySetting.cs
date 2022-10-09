using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Library;

public class LibrarySetting: IdentifiableEntity<LibrarySettingKey>
{
    public string Value { get; private set; }
    public Guid LibraryId { get; private set; }
    public virtual Library Library { get; private set; }

    public LibrarySetting()
    {
    }

    public LibrarySetting(LibrarySettingKey setting, string value) : base(setting)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }

}
