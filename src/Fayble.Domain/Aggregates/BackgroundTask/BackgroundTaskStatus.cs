using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fayble.Domain.Aggregates.BackgroundTask;

[JsonConverter(typeof(StringEnumConverter))]
public enum BackgroundTaskStatus
{
    Queued,
    Running,
    Complete,
    Failed
}