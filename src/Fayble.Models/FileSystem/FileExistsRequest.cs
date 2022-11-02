namespace Fayble.Models.FileSystem;

public class FileExistsRequest
{
    public string FileName { get; }
    public Guid SeriesId { get; }

    public FileExistsRequest(string fileName, Guid seriesId)
    {
        FileName = fileName;
        SeriesId = seriesId;
    }
}