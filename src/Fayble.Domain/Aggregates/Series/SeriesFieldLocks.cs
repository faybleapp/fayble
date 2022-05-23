
namespace Fayble.Domain.Aggregates.Series
{
    public class SeriesFieldLocks
    {
        public Guid SeriesId { get; private set; }
        public bool Name { get; private set; }
        public bool Volume { get; private set; }
        public bool Summary { get; private set; }
        public bool Notes { get; private set; }
        public bool Year { get; private set; }
        public bool Rating { get; private set; }


        public SeriesFieldLocks() { }

        public SeriesFieldLocks(Guid seriesId)
        {
            SeriesId = seriesId;
        }

        public void UpdateLock(string field, bool locked)
        {
            var property = GetType().GetProperty(field);
            property.SetValue(this, locked);
        }
    }
}
