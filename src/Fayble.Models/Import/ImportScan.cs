namespace Fayble.Models.Import;

public class ImportScan
{
    public string Path { get; }

    public ImportScan(string path)
    {
        Path = path;
    }
}