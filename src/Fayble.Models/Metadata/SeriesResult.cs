namespace Fayble.Models.Metadata;

public class SeriesResult
{
    public string Id { get; }
    public string Name { get; }
    public string Image { get; }
    public string Description { get; }
    public string Summary { get; }
    public int StartYear { get; }
    public List<ProviderResult> Providers { get; }
    public List<BookResult> Books { get; }

    public SeriesResult(
        string id,
        string name,
        int startYear,
        string image,
        string description,
        string summary,
        List<ProviderResult> providers,
        List<BookResult> books)
    {
        Books = books;
        Providers = providers;
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        StartYear = startYear;
        Summary = summary;
    }
}