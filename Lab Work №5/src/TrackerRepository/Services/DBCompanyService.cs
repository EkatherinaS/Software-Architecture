using MongoDB.Driver;

namespace TrackerRepository
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
