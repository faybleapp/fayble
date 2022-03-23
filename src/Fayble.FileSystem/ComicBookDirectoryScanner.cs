using Fayble.Models.FileSystem;
using Microsoft.Extensions.Logging;

namespace Fayble.FileSystem;
public static class ComicBookDirectoryScanner
{
    public static IEnumerable<ComicFile> Scan(string directory, List<string> extensions, ILogger logger)
    {
        logger.LogDebug("Scanning path: {Directory}", directory);

        var files = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
            .Where(f => extensions.Contains(Path.GetExtension(f).Replace(".", "").ToLowerInvariant())).ToList();

        var comicFiles = new List<ComicFile>();

        foreach (var file in files)
        {
            try
            {
                logger.LogDebug("Parsing file: {FilePath}", file);
                comicFiles.Add(ComicBookFileParser.Parse(file));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while parsing file. The file may be corrupt or unreadable: {file}", file);
            }
        }

        return comicFiles;
    }
}
