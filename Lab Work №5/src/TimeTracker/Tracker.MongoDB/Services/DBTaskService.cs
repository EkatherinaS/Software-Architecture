using MongoDB.Driver;
using System.Xml.Linq;
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

        public async Task<DBTask> GetByEntity(DBTask task)
        {
            return await collection
                .Find(x => (x.TaskName == task.TaskName &&
                x.TaskDescription == task.TaskDescription &&
                x.Project == task.Project &&
                x.StartTime == task.StartTime &&
                x.StartPosition == task.StartPosition &&
                x.EndTime == task.EndTime &&
                x.EndPosition == task.EndPosition))
                .FirstOrDefaultAsync();
        }
    }
}
