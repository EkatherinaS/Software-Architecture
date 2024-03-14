using Tracker.MongoDB.DBEntities;

namespace Tracker.MongoDB.Services
{
    internal interface IDBUSerService
    {
        public Task<DBUser> getUserByChatIdAsync(long chatId);
        public Task<DBUser> getAdminByCompanyNameAsync(string companyName);
    }
}
