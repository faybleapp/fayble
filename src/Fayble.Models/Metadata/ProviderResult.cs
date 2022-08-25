namespace Fayble.Models.Metadata;

public class ProviderResult
{
    public Guid Id { get; }
    public string ProviderItemId { get;  }
    public string Name { get; }

    public ProviderResult(Guid id, string name, string providerItemId)
    {
        Id = id;
        Name = name;
        ProviderItemId = providerItemId;
    }
}