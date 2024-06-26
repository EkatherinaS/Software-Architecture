﻿using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.Services;

namespace Tracker.MongoDB.DBServices
{
    public abstract class DBService<T>: IDBService<T> where T : DBEntity
    {
        internal IMongoCollection<T> collection;


        public DBService(string collectionName)
        {           
            MongoClient client = new MongoClient(Constants.ConnectionString);
            IMongoDatabase db = client.GetDatabase(Constants.DatabaseName);

            bool alreadyExists = db.ListCollections().ToList().Any(x => x.GetElement("name").Value.ToString() == collectionName);
            if (!alreadyExists)
            {
                db.CreateCollection(collectionName);
            }
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

        public async Task<DeleteResult> DeleteEntity(T entity)
        {
            return await collection.DeleteOneAsync(x => x.Equals(entity));
        }

        public async Task<DeleteResult> DeleteEntity(string id)
        {
            return await collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public List<T> GetAll()
        {
            return collection.AsQueryable().ToList();
        }
    }
}
