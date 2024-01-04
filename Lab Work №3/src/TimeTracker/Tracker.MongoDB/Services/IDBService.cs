namespace Tracker.MongoDB.Services
{
    internal interface IDBService<T>
    {
        public Task<T> GetById(string id);
        public void AddEntity(T entity);
        public Task UpdateEntityAsync(T entity);
        public void DeleteEntity(T entity);
        public Task<List<T>> GetAll();
    }
}