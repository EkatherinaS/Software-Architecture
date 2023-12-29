namespace Tracker.MongoDB.Services
{
    public interface IDBEntityWithNameService<T>
    {
        public Task<T> getByName(string name);
    }
}
