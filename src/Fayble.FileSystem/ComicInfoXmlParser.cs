using System.Xml;
using System.Xml.Serialization;
using Fayble.Models.FileSystem;
using SharpCompress.Archives;

namespace Fayble.FileSystem;

public static class ComicInfoXmlParser
{
    public static ComicInfoXml Parse(string filePath)
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
        return (ComicInfoXml) deserializer.Deserialize(new StringReader(xDoc.InnerXml));
    }
}