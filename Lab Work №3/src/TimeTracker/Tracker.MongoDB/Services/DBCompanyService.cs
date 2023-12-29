using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public class DBCompanyService : DBService<DBCompany>, IDBEntityWithNameService<DBCompany>
    {
        private static string collectionName = "company";
        public DBCompanyService() : base(collectionName) { }

        public async Task<DBCompany> getByName(string name)
        {
            return await collection
                .Find(x => x.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
