namespace Fayble.Models.Import;

public class ImportScanFile
{
    public string FileName { get; }
    public string FilePath { get; }
    public int PageCount { get; }
    public long FileSize { get; }
    public string Number { get; }
    public ComicInfoXml? ComicInfoXml { get; }

    public ImportScanFile(
        string fileName,
        string filePath,
        int pageCount,
        long fileSize,
        string number,
        ComicInfoXml? comicInfoXml)
    {
        FileName = fileName;
        FilePath = filePath;
        PageCount = pageCount;
        FileSize = fileSize;
        Number = number;
        ComicInfoXml = comicInfoXml;
    }
}