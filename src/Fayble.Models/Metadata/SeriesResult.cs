using Fayble.Core.Extensions;

namespace Fayble.Models.Metadata;

public class SeriesResult
{
    public string Id { get; }
    public string Name { get; }
    public string Image { get; }
    public string? Description { get; }
    public string? DescriptionWithoutTags { get; }
    public string Summary { get; }
    public int StartYear { get; }
    public List<ProviderResult> Providers { get; }
    public List<BookSummaryResult>? Books { get; }

    public SeriesResult(
        string id,
        string name,
        int startYear,
        string image,
        string? description,
        string summary,
        List<ProviderResult> providers,
        IEnumerable<BookSummaryResult>? books)
    {
        Books = books?.OrderByAlphaNumeric(b => b.Number).ToList();
        Providers = providers;
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        StartYear = startYear;
        Summary = summary;
        DescriptionWithoutTags = description?.RemoveHtmlTags();
    }
}