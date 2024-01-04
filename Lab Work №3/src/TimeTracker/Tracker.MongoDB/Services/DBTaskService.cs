using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public class DBTaskService : DBService<DBTask>, IDBEntityWithNameService<DBTask>
    {
        private static string collectionName = "task";
        public DBTaskService() : base(collectionName) { }

        public async Task<DBTask> getByName(string name)
        {
            return await collection
                .Find(x => x.TaskName == name)
                .FirstOrDefaultAsync();
        }
    }
}
