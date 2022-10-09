using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.MediaSetting;

namespace Fayble.Domain.Repositories;

public interface IMediaSettingRepository : IRepositoryBase<MediaSetting, MediaSettingKey>
{
}