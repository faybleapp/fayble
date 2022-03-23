using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fayble.Domain.Aggregates.BackgroundTask;

public enum BackgroundTaskType
{
    LibraryScan,
    SeriesScan
}