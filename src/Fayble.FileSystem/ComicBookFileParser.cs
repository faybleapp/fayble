using System.Text.RegularExpressions;
using Fayble.Core.Helpers;
using Fayble.Models.FileSystem;
using SharpCompress.Archives;

namespace Fayble.FileSystem
{
    public static class ComicBookFileParser
    {
        public static ComicFile Parse(string filePath)
        {
            var pageCount = GetPageCount(filePath);
            var fileName = Path.GetFileName(filePath);
            var series = ParseSeries(fileName).Trim();
            var number = ParseIssueNumber(fileName);
            var volume = ParseVolume(fileName).VolumeNumber;
            var year = CleanNumeric(ParseYear(fileName));
            var fileFormat = Path.GetExtension(fileName);

            //TODO: Check library settings to see if ComicInfo should be checked.
            var comicInfoXml = ComicInfoXmlParser.Parse(filePath);

            var comicFile = new ComicFile(series, number, year, volume, fileFormat, filePath, null, fileName, pageCount, comicInfoXml);

            return comicFile;
        }

        public static void ExtractComicCoverImage(string filePath, string mediaPath)
        {
            using var archive = ArchiveFactory.Open(filePath);
            var image =
                archive.Entries.Where(
                    x =>
                        x.Key.ToLower().EndsWith(".jpg") || x.Key.ToLower().EndsWith(".jpeg") ||
                        x.Key.ToLower().EndsWith(".png") ||
                        x.Key.ToLower().EndsWith(".bmp")).OrderBy(x => x.Key).FirstOrDefault();

            var path = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath, "cover.jpg");

            image.WriteToFile(path);
            ImageHelpers.ResizeImage(path, 250);
            ImageHelpers.ResizeImage(path, 500);
        }

        private static int GetPageCount(string filePath)
        {
            using var archive = ArchiveFactory.Open(filePath);
            var images =
                archive.Entries.Where(
                    x =>
                        x.Key.ToLower().EndsWith(".jpg") || x.Key.ToLower().EndsWith(".jpeg") ||
                        x.Key.ToLower().EndsWith(".png") ||
                        x.Key.ToLower().EndsWith(".bmp"));

            return images.Count();
        }

        private static string CleanNumeric(string value)
        {
            //Clean string and remove non numeric characters
            var digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(value, "");
        }

        private static string ParseYear(string fileName)
        {
            var year = "";

            //Try year in parentheses
            var yearWithParenthesis = new Regex(@"\(\b(19|20)\d{2}\b\)");

            var match = yearWithParenthesis.Match(fileName);
            if (match.Success)
            {
                year = match.Value;
                return year;
            }

            //Try year in any punctuation, whitespace or start or end of string
            var yearWithoutParenthesis = new Regex(@"\b(19|20)\d{2}\b");

            match = yearWithoutParenthesis.Match(fileName);
            if (match.Success)
            {
                year = match.Value;
                return year;
            }

            //Try year anywhere in string
            var yearAnywhere = new Regex(@"(19|20)\d{2}");

            match = yearAnywhere.Match(fileName);
            if (!match.Success) return year;
            year = match.Value;
            return year;
        }

        private static string ParseSeries(string fileName)
        {
            var series = "";

            //Get text before first parentheses
            if (fileName.Contains("("))
            {
                series = fileName.Split('(')[0];
            }

            //Remove volume information if not caught above
            var (volume, _) = ParseVolume(series);
            if (!string.IsNullOrWhiteSpace(volume))
            {
                series = series.Replace(volume, "");
            }

            //Remove year if not caught above
            var year = ParseYear(series);
            if (!string.IsNullOrWhiteSpace(year))
            {
                series = series.Replace(year, "");
            }

            //Remove issue number if not caught above
            var number = ParseIssueNumber(series);
            if (!string.IsNullOrWhiteSpace(number))
            {
                series = series.Replace(number, "");
            }

            //Remove any remaining # symbols
            if (series.Contains('#'))
            {
                series = series.Replace("#", "");
            }

            //Replace dashes with colons
            if (series.Contains(" - "))
            {
                series = series.Replace(" - ", ": ");
            }

            return series;
        }

        private static string ParseIssueNumber(string fileName)
        {
            string number = "";
            Regex regex;
            Match match;

            //Issue number containing dots, eg Amazing Spider Man 001.5
            regex = new Regex(@"\d{1,3}[,.]\d{1,3}");

            match = regex.Match(fileName);
            if (match.Success)
            {
                number = match.Value;
                return number;
            }

            //Issue number between 1 and 3 digits immediately following a #
            regex = new Regex(@"[#]\d{1,3}");

            match = regex.Match(fileName);
            if (match.Success)
            {
                number = match.Value;
                number = number.Replace("#", "");

                return number;
            }

            //Issue number in standard 4 digit format, eg. 1000, 1001, 1052 and starts with a 1
            //This caters for Action Comics and Detective Comics
            regex = new Regex(@"\b\d{4}\b");

            match = regex.Match(fileName);
            if (match.Success)
            {
                number = match.Value;
                //For now we assume no series in higher than 1899 issues
                if (!number.StartsWith("19") && !number.StartsWith("20"))
                {
                    return number;
                }
            }

            //Issue number in standard 3 digit format, eg. 003, 041, 052
            regex = new Regex(@"\b\d{3}\b");

            match = regex.Match(fileName);
            if (match.Success)
            {
                number = match.Value;

                return number;
            }

            //Issue number in 2 digit format with whitespace, eg. 02, 10, 13
            regex = new Regex(@"\s\d{2}\s");

            match = regex.Match(fileName);
            if (!match.Success) return number;
            number = match.Value.Trim().PadLeft(3, '0');
            return number;
        }

        private static (string Volume, string VolumeNumber) ParseVolume(string fileName)
        {
            Regex regex;
            Match match;

            //Find Vol ## eg. Vol 01, Vol 04
            regex = new Regex(@"Vol \d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Vol ", "", RegexOptions.IgnoreCase));
            }

            //Find Vol## eg, Vol01, Vol04
            regex = new Regex(@"Vol\d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Vol", "", RegexOptions.IgnoreCase));
            }

            //Find Vol## eg, Vol.1, Vol.04
            regex = new Regex(@"Vol.\d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Vol.", "", RegexOptions.IgnoreCase));
            }

            //Find Vol## eg, Vol. 1, Vol. 04
            regex = new Regex(@"Vol. \d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Vol. ", "", RegexOptions.IgnoreCase));
            }

            //Find Volume## eg, Volume01, Vol04
            regex = new Regex(@"Volume\d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Volume", "", RegexOptions.IgnoreCase));
            }

            //Find Volume ## eg, Volume 01, Volume 04
            regex = new Regex(@"Volume \d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "Volume ", "", RegexOptions.IgnoreCase));
            }

            //Find v## eg, v01, v04
            regex = new Regex(@"V\d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "v", "", RegexOptions.IgnoreCase));
            }

            //Find v ## eg, v 01, v 04
            regex = new Regex(@"V \d{1,4}", RegexOptions.IgnoreCase);

            match = regex.Match(fileName);
            if (match.Success)
            {
                return (match.Value, Regex.Replace(match.Value, "v ", "", RegexOptions.IgnoreCase));
            }

            return (null, null);
        }
    }
}