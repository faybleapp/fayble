using System.Xml;
using System.Xml.Serialization;
using Fayble.Core.Helpers;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Microsoft.Extensions.Logging;
using SharpCompress.Archives;

namespace Fayble.Services.FileSystem;

public class ComicBookFileSystemService : FileSystemService, IComicBookFileSystemService
{
    private readonly IFileTypeRepository _fileTypeRepository;
    private readonly ILogger _logger;

    public ComicBookFileSystemService(
        IFileTypeRepository fileTypeRepository,
        ILogger<ComicBookFileSystemService> logger) : base(fileTypeRepository)
    {
        _fileTypeRepository = fileTypeRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetSeriesDirectories(string libraryPath)
    {
        var extensions = (await _fileTypeRepository.Get(x => x.MediaType == MediaType.ComicBook))
            .Select(x => x.FileExtension).ToList();

        return Directory.EnumerateFiles(libraryPath, "*.*", SearchOption.AllDirectories).Where(
                f => extensions.Contains(Path.GetExtension(f).Replace(".", "").ToLowerInvariant()))
            .Select(f => new FileInfo(f).DirectoryName).Distinct()!;
    }

    public int GetPageCount(string filePath)
    {
        using var archive = ArchiveFactory.Open(filePath);
        var images =
            archive.Entries.Where(
                x =>
                    x.Key.ToLower().EndsWith(".jpg") || 
                    x.Key.ToLower().EndsWith(".jpeg") ||
                    x.Key.ToLower().EndsWith(".png") ||
                    x.Key.ToLower().EndsWith(".bmp"));

        return images.Count();
    }

    public void ExtractComicCoverImage(string filePath, string mediaPath)
    {
        using var archive = ArchiveFactory.Open(filePath);
        var image =
            archive.Entries.Where(
                x =>
                    x.Key.ToLower().EndsWith(".jpg") || x.Key.ToLower().EndsWith(".jpeg") ||
                    x.Key.ToLower().EndsWith(".png") ||
                    x.Key.ToLower().EndsWith(".bmp")).OrderBy(x => x.Key).FirstOrDefault();
    
        var folderPath = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    
        var path = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath, "cover.jpg");
    
        image.WriteToFile(path);
        ImageHelpers.ResizeImage(path, 250);
        ImageHelpers.ResizeImage(path, 500);
    }
    
    public ComicInfoXml? ReadComicInfoXml(string filePath)
    {
        using var archive = ArchiveFactory.Open(filePath);
        var comicInfo = archive.Entries.FirstOrDefault(x => x.Key.ToLower() == "comicinfo.xml");
    
        if (comicInfo == null) return null;
    
        using var xmlStream = comicInfo.OpenEntryStream();
        var xml = new StreamReader(xmlStream)?.ReadToEnd();
        if (xml == null) return null;
        var deserializer = new XmlSerializer(typeof(ComicInfoXml), new XmlRootAttribute("ComicInfo"));
        var xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        return (ComicInfoXml)deserializer.Deserialize(new StringReader(xDoc.InnerXml));
    }
  
}