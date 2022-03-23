using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fayble.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum MediaType
{
    ComicBook,
    Book
}
