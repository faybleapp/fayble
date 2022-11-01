namespace Fayble.Models.Import;

public class GenerateFilenameRequest
{
    public Guid SeriesId { get; private set; }
    public string Number { get;  private set; }
    public Guid? BookMatchId { get; set; }
    

    public GenerateFilenameRequest(Guid seriesId, string number, Guid? bookMatchId)
    {
        SeriesId = seriesId;
        Number = number;
        BookMatchId = bookMatchId;
    }
}