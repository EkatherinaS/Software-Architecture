using Tracker.MongoDB.DBEntities;

namespace Tracker.MongoDB.Models
{
    public class DBGPS : DBEntity
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
