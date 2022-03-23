using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fayble.Domain.Aggregates.Configuration;

[JsonConverter(typeof(StringEnumConverter))]
public enum Setting
{
    ReviewOnImport
}
