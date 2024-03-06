using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public class DBProjectService : DBService<DBProject>, IDBEntityWithNameService<DBProject>
    {
        private static string collectionName = "project";
        public DBProjectService() : base(collectionName) { }

        public async Task<DBProject> getByName(string name)
        {
            return await collection
                .Find(x => x.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
