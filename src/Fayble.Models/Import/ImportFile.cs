using Fayble.Models.FileSystem;

namespace Fayble.Models.Import;

public class ImportFile
{
    public Guid? SeriesId { get; private set; }
    public string DestinationFileName {get; private set;}
    public string FilePath { get; private set; }
    public string Number { get; private set; }
    public ImportFile(
        Guid? seriesId,
        string destinationFileName,
        string filePath,
        string number)
    {
        SeriesId = seriesId;
        DestinationFileName = destinationFileName;
        FilePath = filePath;
        Number = number;
    }
}