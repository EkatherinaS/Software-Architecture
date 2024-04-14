using MongoDB.Driver;

namespace TrackerRepository
{
    public class DBCompanyService : DBService<DBCompany>, IDBEntityWithNameService<DBCompany>
    {
        private static string collectionName = "company";
        private DBCompanyService() : base(collectionName) { }

        private static volatile DBCompanyService instance;
        private static object syncRoot = new Object();

        public static DBCompanyService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DBCompanyService();
                    }
                }

                return instance;
            }
        }

        public async Task<DBCompany> getByName(string name)
        {
            return await collection
                .Find(x => x.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
