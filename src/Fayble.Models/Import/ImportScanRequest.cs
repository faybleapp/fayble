namespace Fayble.Models.Import;

public class ImportScanRequest
{
    public string Path { get; }

    public ImportScanRequest(string path)
    {
        Path = path;
    }
}