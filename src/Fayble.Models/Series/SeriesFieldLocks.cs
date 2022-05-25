namespace Fayble.Models.Series;

public class SeriesFieldLocks
{
    public bool Name { get; }
    public bool Volume { get; }
    public bool Summary { get; }
    public bool Notes { get; }
    public bool Year { get; }
    public bool Rating { get; }
    public bool PublisherId { get; }
    public bool ParentSeriesId { get; }

    public SeriesFieldLocks(
        bool name,
        bool volume,
        bool summary,
        bool notes,
        bool year,
        bool rating,
        bool publisherId,
        bool parentSeriesId)
    {
        Name = name;
        Volume = volume;
        Summary = summary;
        Notes = notes;
        Year = year;
        Rating = rating;
        PublisherId = publisherId;
        ParentSeriesId = parentSeriesId;
    }
}