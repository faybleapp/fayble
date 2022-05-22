using Fayble.Models.Book;
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
            entity.File.PageCount,
            entity.File.FileName,
            entity.File.FileType,
            Math.Round(Convert.ToDouble(entity.File.FileSize / 1024) / 1024, 2),
            Path.Combine(entity.Library.FolderPath, entity.File.FilePath),
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
            entity.Tags?.Select(t => t.Name).OrderBy(t => t),
            entity.DeletedDate != null,
            entity.MediaRoot,
            entity.People?.Select(p => p.ToModel()),
            entity.FieldLocks.ToModel()
            );
    }

    public static BookFieldLocks ToModel(this Domain.Aggregates.Book.BookFieldLocks entity)
    {
        return new BookFieldLocks(
            entity.CoverDate,
            entity.Language,
            entity.Number,
            entity.Rating,
            entity.ReleaseDate,
            entity.Summary,
            entity.Title, 
            entity.Tags);
    }

    private static BookPerson ToModel(this Domain.Aggregates.BookPerson entity)
    {
        return new BookPerson(entity.PersonId, entity.Person.Name, entity.Role.ToString());
    }
}