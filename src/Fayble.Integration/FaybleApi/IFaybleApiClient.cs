﻿using Fayble.Models.Metadata;

namespace Fayble.Integration.FaybleApi;

public interface IFaybleApiClient
{
    Task<IEnumerable<SeriesSearchResult>> SearchSeries(string name, int? year);
    Task<SeriesResult> GetSeries(Guid id);
}