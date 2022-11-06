namespace Fayble.Models.Metadata;

public class BookResult
{
    public Guid Id { get; }
    public string? Title { get; }
    public string? Number { get; }
    public string? Summary { get; }
    public DateTime? ReleaseDate { get; }
    public DateTime? CoverDate { get; }
    public string? Image { get; }
    public Guid? SeriesId { get; }
    public List<ProviderResult>? Providers { get; }

    public BookResult(
        Guid id,
        string? title,
        string? number,
        string? summary,
        DateTime? releaseDate,
        DateTime? coverDate,
        string? image,
        Guid? seriesId,
        List<ProviderResult>? providers)
    {
        Id = id;
        Title = title;
        Number = number;
        Summary = summary;
        ReleaseDate = releaseDate;
        CoverDate = coverDate;
        Image = image;
        SeriesId = seriesId;
        Providers = providers;
    }
}