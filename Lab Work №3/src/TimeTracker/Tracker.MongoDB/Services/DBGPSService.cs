using Tracker.MongoDB.Models;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public class DBGPSService : DBService<DBGPS>
    {
        private static string collectionName = "gps";
        public DBGPSService() : base(collectionName) { }
    }
}
