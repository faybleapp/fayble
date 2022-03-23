namespace Fayble.Models.Series;

public class UpdateSeries
{
    public Guid Id { get; }
    public string? Name { get; }
    public int? Year { get; }
    public string? Summary { get; }
    public string? Notes { get; }
    public string? Volume { get; }
    public decimal Rating { get; }
    public Guid? PublisherId { get; }
    public Guid? ParentSeriesId { get; }

    public UpdateSeries(
        Guid id,
        string? name,
        int? year,
        string? summary,
        string? notes,
        string? volume,
        decimal rating,
        Guid? publisherId,
        Guid? parentSeriesId)
    {
        Id = id;
        Name = name;
        Year = year;
        Summary = summary;
        Notes = notes;
        Volume = volume == string.Empty ? null : volume;
        Rating = rating;
        PublisherId = publisherId;
        ParentSeriesId = parentSeriesId;

    }

}