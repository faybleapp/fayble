namespace Fayble.Models.Import;

public class GenerateFilenameResponse
{
    public string FileName { get; private set; }
    public bool Exists { get; private set; }

    public GenerateFilenameResponse(string fileName, bool exists)
    {
        FileName = fileName;
        Exists = exists;
    }
}