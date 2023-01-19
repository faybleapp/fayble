using Fayble.Core.Extensions;
using Fayble.Core.Helpers;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Models.Import;
using Fayble.Models.Metadata;
using Fayble.Models.Settings;
using Fayble.Services.MetadataService;
using Fayble.Services.Settings;
using Microsoft.Extensions.Logging;
using SharpCompress.Archives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Fayble.Services.FileSystem;

public class ComicBookFileSystemService : FileSystemService, IComicBookFileSystemService
{
    private readonly IFileTypeRepository _fileTypeRepository;
    private readonly ILogger _logger;
    private readonly ISeriesRepository _seriesRepository;
    private readonly ISettingsService _settingsService;
    private readonly IMetadataService _metadataService;

    public ComicBookFileSystemService(
        IFileTypeRepository fileTypeRepository,
        ILogger<ComicBookFileSystemService> logger,
        ISeriesRepository seriesRepository,
        ISettingsService settingsService,
        IMetadataService metadataService) : base(fileTypeRepository)
    {
        _fileTypeRepository = fileTypeRepository;
        _logger = logger;
        _seriesRepository = seriesRepository;
        _settingsService = settingsService;
        _metadataService = metadataService;
    }

    public async Task<IEnumerable<string>> GetSeriesDirectories(string libraryPath)
    {
        var extensions = (await _fileTypeRepository.Get(x => x.MediaType == MediaType.ComicBook))
            .Select(x => x.FileExtension).ToList();

        return Directory.EnumerateFiles(libraryPath, "*.*", SearchOption.AllDirectories).Where(
                f => extensions.Contains(Path.GetExtension(f).Replace(".", "").ToLowerInvariant()))
            .Select(f => new FileInfo(f).DirectoryName).Distinct()!;
    }

    public void ExtractComicCoverImage(string filePath, string mediaRoot, Guid id)
    {
        using var archive = ArchiveFactory.Open(filePath);
        var extractedImage =
            archive.Entries.Where(
                x =>
                    x.Key.ToLower().EndsWith(".jpg") || x.Key.ToLower().EndsWith(".jpeg") ||
                    x.Key.ToLower().EndsWith(".png") ||
                    x.Key.ToLower().EndsWith(".bmp")).MinBy(x => x.Key);
    
        var folderPath = Path.Combine(ApplicationHelpers.GetMediaDirectory(), mediaRoot, id.ToString());
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (extractedImage == null)
            return;

        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        extractedImage.WriteToFile(tempFile);
        
        using var img = Image.Load(tempFile);
        var path = Path.Combine(folderPath, "cover.jpg");
        ImageHelpers.SaveImage(img, path, 1000);
        ImageHelpers.SaveImage(img, path, 250);
        ImageHelpers.SaveImage(img, path, 500);
        File.Delete(tempFile);
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
        return deserializer.Deserialize(new StringReader(xDoc.InnerXml)) as ComicInfoXml;
    }

    public ComicFile GetFile(string filePath, bool getPages = true)
    {
        _logger.LogDebug($"Retrieving file: {filePath}");
        var file = new FileInfo(filePath);

        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var fileSize = file.Length;
        var lastModified = file.LastWriteTimeUtc;
        var fileExtension = Path.GetExtension(filePath);
        var comicInfoXml = ReadComicInfoXml(filePath);

        return new ComicFile(
            fileExtension,
            filePath,
            null,
            fileName,
            fileSize,
            lastModified,
            comicInfoXml,
            getPages ? GetPages(filePath) : new List<ComicPage>());
    }

    private List<ComicPage> GetPages(string filePath)
    {
        var pages = new List<ComicPage>();
        var archive = ArchiveFactory.Open(filePath);
        var imageEntries =
            archive.Entries.Where(
                x =>
                    x.Key.ToLower().EndsWith(".jpg") ||
                    x.Key.ToLower().EndsWith(".jpeg") ||
                    x.Key.ToLower().EndsWith(".png") ||
                    x.Key.ToLower().EndsWith(".bmp")).OrderBy(x => x.Key);

        var index = 1;
        foreach (var imageEntry in imageEntries)
        {
            using var img = Image.Load(imageEntry.OpenEntryStream(), out IImageFormat imageFormat);
            pages.Add(new ComicPage(img.Width, img.Height, imageEntry.Key, imageEntry.Size, index, imageFormat.DefaultMimeType));
            index++;
        }

        return pages;
    }

    public async Task<bool> FileExists(FileExistsRequest request)
    {
        var series = await _seriesRepository.Get(request.SeriesId);
        return series.Books.Any(b => b.File.FileName.ToLower() == request.FileName.ToLower());
    }

    public async Task<string> GenerateFilename(GenerateFilenameRequest request)
    {
        var series = await _seriesRepository.Get(request.SeriesId);
        var mediaSettings = await _settingsService.GetMediaSettings();
        var colonReplacement = ColonReplacement.GetColonReplacementValue(mediaSettings.ColonReplacement);
        var missingTokenReplacement = MissingTokenReplacement.GetMissingTokenReplacementValue(mediaSettings.MissingTokenReplacement);
        var filename = mediaSettings.ComicBookStandardNamingFormat;

        BookResult? metadata = null;
        if (request.BookMatchId != null)
        {
            metadata = await _metadataService.GetBook((Guid)request.BookMatchId);
        }

        filename = filename.Replace(
            FilenameTokens.SeriesName,
            series.Name.RemoveIllegalCharacters(colonReplacement),
            StringComparison.InvariantCultureIgnoreCase);

        filename = filename.Replace(FilenameTokens.SeriesYear, series.Year.ToString() ?? missingTokenReplacement);
        filename = filename.Replace(FilenameTokens.SeriesVolume, series.Volume ?? missingTokenReplacement);
        filename = filename.Replace(
            FilenameTokens.BookCoverDateShort,
            metadata?.CoverDate?.ToString("yyyy-MM") ?? missingTokenReplacement);
        filename = filename.Replace(
            FilenameTokens.BookCoverDateLong,
            metadata?.CoverDate?.ToString("MMM yyyy") ?? missingTokenReplacement);
        filename = filename.Replace(
            FilenameTokens.BookCoverDateLongComma,
            metadata?.CoverDate?.ToString("MMM, yyyy") ?? missingTokenReplacement);
        filename = filename.Replace(
            FilenameTokens.BookCoverDateFull,
            metadata?.CoverDate?.ToString("MMMM yyyy") ?? missingTokenReplacement);
        filename = filename.Replace(
            FilenameTokens.BookCoverDateFullComma,
            metadata?.CoverDate?.ToString("MMMM, yyyy") ?? missingTokenReplacement);

        var regex = new Regex(FilenameTokens.BookNumberPadding);
        var bookNumberTokens = regex.Matches(filename);
        foreach (Match bookNumberToken in bookNumberTokens)
        {
            filename = filename.Replace(
                bookNumberToken.Groups[0].Value,
                request.Number?.PadLeft(bookNumberToken.Groups[1].Value.TrimStart(':').Length, '0'));
        }

        filename = filename.Replace("()", string.Empty);
        filename = filename.Replace("[]", string.Empty);
        filename = filename.Trim();
        
        return filename;
    }
}