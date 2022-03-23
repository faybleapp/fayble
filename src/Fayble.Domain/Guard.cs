using Fayble.Core.Exceptions;

namespace Fayble.Domain;

public class Guard
{
    public static void AgainstNullOrWhitespace(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{propertyName} must be not null and not an empty string.");
    }

    public static void AgainstEmpty(Guid? guid, string propertyName)
    {
        if (guid.HasValue && guid == Guid.Empty) throw new DomainException($"{propertyName} cannot be empty.");
    }
}