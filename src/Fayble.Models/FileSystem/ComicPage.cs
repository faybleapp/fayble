using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Fayble.Models.FileSystem;

public class ComicPage
{
    public int Width { get; }
    public int Height { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public string? FileHash { get; }
    public int Number { get; }
    public string MediaType { get; }


    public ComicPage(int width, int height, string fileName, long fileSize, int number, string mediaType, string fileHash = null)
    {
        Width = width;
        Height = height;
        FileName = fileName;
        FileSize = fileSize;
        FileHash = fileHash;
        Number = number;
        MediaType = mediaType;
    }
}