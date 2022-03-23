using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Book;

public class ReadHistory : IdentifiableEntity<Guid>
{
    public Guid BookId { get; private set; }

    public DateTimeOffset ReadDate { get; private set; }

    public Guid UserId { get; private set; }

    public virtual User.User User { get; private set; }


    ReadHistory()
    {
    }

    public ReadHistory(Guid bookId, Guid userId, DateTimeOffset? readDate = null)
    {
        BookId = bookId;
        ReadDate = readDate ?? DateTimeOffset.Now;
        UserId = userId;
    }
}