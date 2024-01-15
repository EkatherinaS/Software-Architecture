using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public class DBUserService : DBService<DBUser>, IDBUSerService, IDBEntityWithNameService<DBUser>
    {
        private static string collectionName = "user";
        public DBUserService() : base(collectionName) { }

        public async Task<DBUser> getUserByChatIdAsync(long chatId)
        {
                return await collection
                    .Find(x => x.IdChat == chatId)
                    .FirstOrDefaultAsync();
        }

        public async Task<DBUser> getAdminByCompanyNameAsync(string companyName)
        {
            return await collection
                .Find(x => x.IsAdmin == true && x.Company == companyName)
                .FirstOrDefaultAsync();
        }

        public async Task<DBUser> getByName(string name)
        {
            return await collection
                .Find(x => x.Nickname == name)
                .FirstOrDefaultAsync();
        }
    }
}
