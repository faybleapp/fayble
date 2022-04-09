using Fayble.Services.Library;
using Fayble.Services.Publisher;
using Fayble.Services.Series;

namespace Fayble.Services.Book;

public static class Mappings
{
    public static Models.Book.Book ToModel(this Domain.Aggregates.Book.Book entity, Guid? userId = null)
    {
        return new Models.Book.Book(
            entity.Id,
            entity.Title,
            entity.Summary,
            entity.PageCount,
            entity.MediaPath,
            entity.Filename,
            entity.FileFormat,
            entity.Rating,
            entity.Publisher?.ToModel(),
            userId != null && entity.IsRead((Guid) userId),
            entity.Number,
            entity.Series?.ToModel(userId),
            entity.Library?.ToModel(),
            entity.MediaType.ToString(),
            entity.ReleaseDate?.ToString("yyyy-MM-dd"),
            entity.CoverDate?.ToString("yyyy-MM"),
            entity.Language,
            entity.Tags?.Select(t => t.Name).OrderBy(t => t));
    }
}