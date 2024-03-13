using MongoDB.Driver;

namespace Tracker.MongoDB.Services
{
    internal interface IDBService<T>
    {
        public Task<T> GetById(string id);
        public void AddEntity(T entity);
        public Task UpdateEntityAsync(T entity);
        public Task<DeleteResult> DeleteEntity(T entity);
        public List<T> GetAll();
    }
}