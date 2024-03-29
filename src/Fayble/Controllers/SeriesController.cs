﻿using Fayble.Models.Series;
using Fayble.Services.Series;
using Microsoft.AspNetCore.Mvc;
using Book = Fayble.Models.Book.Book;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeriesController : ControllerBase
{
    private readonly ISeriesService _seriesService;

    public SeriesController(ISeriesService seriesService)
    {
        _seriesService = seriesService;
    }

    [HttpGet("{id}")]
    public async Task<Series?> Get(Guid id)
    {
        return await _seriesService.Get(id);
    }

    [HttpGet]
    public async Task<IEnumerable<Series>?> GetAll()
    {
        return await _seriesService.GetAll();
    }

    [HttpGet("{id}/books")]
    public async Task<IEnumerable<Book>?> Books(Guid id)
    {
        return await _seriesService.GetBooks(id);
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult<Series>> Update(Guid id, [FromBody] UpdateSeries series)
    {
        return await _seriesService.Update(id, series);
    }

}
