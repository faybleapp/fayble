namespace Fayble.Domain.Aggregates.Library;

public class LibrarySetting
{
    public LibrarySettingKey Setting { get; private set; }
    public string Value { get; private set; }
    public Guid LibraryId { get; private set; }
    public virtual Library Library { get; private set; }

    public LibrarySetting()
    {
    }

    public LibrarySetting(LibrarySettingKey setting, string value)
    {
        Value = value;
        Setting = setting;
    }

    public void Update(string value)
    {
        Value = value;
    }

}
