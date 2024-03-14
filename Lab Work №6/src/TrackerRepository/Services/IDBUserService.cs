namespace TrackerRepository
{
    internal interface IDBUSerService
    {
        public Task<DBUser> getUserByChatIdAsync(long chatId);
        public Task<DBUser> getAdminByCompanyNameAsync(string companyName);
    }
}
