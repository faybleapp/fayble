﻿namespace Fayble.Services.Publisher;

public static class Mappings
{
    public static Models.Publisher.Publisher ToModel(this Domain.Aggregates.Publisher.Publisher entity)
    {
        return new Models.Publisher.Publisher(entity.Id, entity.Name, entity.Description, entity.MediaPath);
    }

}
