using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class MediaSettingRepository : RepositoryBase<FaybleDbContext, MediaSetting, MediaSettingKey>, IMediaSettingRepository
{
    public MediaSettingRepository(FaybleDbContext context) : base(context)
    {
    }
}