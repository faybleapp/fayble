using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fayble.Core.Extensions;

namespace Fayble.Models.Metadata;
public class SeriesSearchResult
{
    public Guid Id { get; }
    public string? Name { get; }
    public string? Summary { get; }
    public string? Description { get; }
    public int? StartYear { get; }
    public string? Publisher { get; }
    public int IssueCount { get; }
    public string? Image { get; }
    public int LevenshteinDistance { get; }


    public SeriesSearchResult(
        Guid id,
        string? name,
        int? startYear,
        string? summary,
        string? description,
        string? publisher,
        int issueCount,
        string? image,
        int levenshteinDistance)
    {
        Id = id;
        Name = name;
        StartYear = startYear;
        Publisher = publisher;
        Summary = summary;
        IssueCount = issueCount;
        Image = image;
        LevenshteinDistance = levenshteinDistance;
        Description = description?.RemoveHtmlTags();
    }
}
