using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public abstract class DBService<T>: IDBService<T> where T : DBEntity
    {
        internal IMongoCollection<T> collection;


        public DBService(string collectionName)
        {
            Constants.SetUp();
            MongoClient client = new MongoClient(Constants.ConnectionString);
            IMongoDatabase db = client.GetDatabase(Constants.DatabaseName);
            collection = db.GetCollection<T>(collectionName);
        }

        public async Task<T> GetById(string id)
        {
            return await collection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async void AddEntity(T entity)
        {
            if (GetById(entity.Id).Result == null)
            {
                await collection.InsertOneAsync(entity);
            }
        }

        public async Task UpdateEntityAsync(T entity)
        {
            if (entity != null)
            {
                await collection
                    .ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity);
            }
        }

        public async void DeleteEntity(T entity)
        {
            if (GetById(entity.Id).Result != null)
            {
                await collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await collection.AsQueryable().ToListAsync();
        }
    }
}
