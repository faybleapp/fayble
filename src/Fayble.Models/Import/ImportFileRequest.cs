namespace Fayble.Models.Import;

public class ImportFileRequest
{
    public Guid SeriesId { get; }
    public string DestinationFileName { get; }
    public string FilePath { get; }
    public string Number { get; }
    public Guid? MatchId { get; }

    public ImportFileRequest(
        Guid seriesId,
        string destinationFileName,
        string filePath,
        string number,
        Guid? matchId)
    {
        SeriesId = seriesId;
        DestinationFileName = destinationFileName;
        FilePath = filePath;
        Number = number;
        MatchId = matchId;
    }
}