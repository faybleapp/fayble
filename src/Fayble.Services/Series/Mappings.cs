
using Fayble.Models.Series;
using Fayble.Services.Library;
using Fayble.Services.Publisher;

namespace Fayble.Services.Series;

public static class Mappings
{
    public static Models.Series.Series ToModel(this Domain.Aggregates.Series.Series entity, Guid? userId = null)
    {
        return new Models.Series.Series(entity.Id,
            entity.Name,
            entity.Volume,
            entity.Summary,
            entity.Notes,
            entity.Year,
            entity.BookCount,
            entity.ParentSeriesId,
            entity.ParentSeries?.ToModel(userId),
            entity.PublisherId,
            entity.Publisher?.ToModel(),
            entity.Rating,
            entity.MediaRoot,
            entity.Library.ToModel(),
            userId != null && entity.IsRead((Guid)userId),
            entity.Locked, entity.FieldLocks.ToModel());
    }

    public static SeriesFieldLocks ToModel(this Domain.Aggregates.Series.SeriesFieldLocks entity)
    {
        return new SeriesFieldLocks(
            entity.Name,
            entity.Volume,
            entity.Summary,
            entity.Notes,
            entity.Year,
            entity.Rating);
    }
}

