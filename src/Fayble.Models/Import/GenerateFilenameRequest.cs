namespace Fayble.Models.Import;

public class GenerateFilenameRequest
{
    public Guid SeriesId { get; }
    public string Number { get; }
    public Guid? BookMatchId { get; }

    public GenerateFilenameRequest(Guid seriesId, string number, Guid? bookMatchId)
    {
        SeriesId = seriesId;
        Number = number;
        BookMatchId = bookMatchId;
    }
}