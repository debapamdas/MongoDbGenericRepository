using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbGenericRepository.Abstractions;
using MongoDbGenericRepository.Attributes;

namespace MongoDbGenericRepository
{
    public abstract class BaseMongoRepository : IRepository
    {
        private readonly IMongoClient _client;
        private IMongoDatabase _db;

        protected BaseMongoRepository()
        {
            
        }
        protected BaseMongoRepository(string connectionString, string databaseName): this(new MongoClient(connectionString), databaseName)
        {
        }
        protected BaseMongoRepository(IMongoClient client, string databaseName)
        {
            _client = client;
            _db = client.GetDatabase(databaseName);
        }

        protected BaseMongoRepository(IMongoDatabase db)
        {
            _db = db;
            _client = db.Client;
        }

        public async Task<TEntity> Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : IEntity
        {
            try
            {
                return await GetCollection<TEntity>().Find(expression).FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : IEntity
        {
            try
            {
                // return await All().Where(exp).ToListAsync();
                var list = new List<TEntity>();
                using (var cursor = await GetCollection<TEntity>().FindAsync(expression))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        list.AddRange(cursor.Current);
                    }
                }
                return list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task Add<TEntity>(TEntity item) where TEntity : IEntity
        {
            try
            {
                await GetCollection<TEntity>().InsertOneAsync(item);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> Update<TEntity>(Expression<Func<TEntity, bool>> expression, TEntity item) where TEntity : IEntity
        {
            try
            {
                var updateResult = await GetCollection<TEntity>().ReplaceOneAsync(expression, item);
                return updateResult.ModifiedCount;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> Delete<TEntity>(Expression<System.Func<TEntity, bool>> expression) where TEntity : IEntity
        {
            try
            {
                var deleteResult = await GetCollection<TEntity>().DeleteManyAsync(expression);
                return deleteResult.DeletedCount;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> DeleteAll<TEntity>() where TEntity : IEntity
        {
            try
            {
                var deleteResult = await GetCollection<TEntity>().DeleteManyAsync(_ => true);
                return deleteResult.DeletedCount;
            }
            catch (System.Exception ex)
            {
                
                throw ex;
            }
        }
        private IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IEntity
        {
            Type type = typeof(TEntity);
            var attr = type.GetCustomAttribute(typeof(CollectionNameAttribute)) as CollectionNameAttribute;
            string collectionName = attr._collectionName;
            if (collectionName != null)
            {
                return _db.GetCollection<TEntity>(collectionName);
            }
            return _db.GetCollection<TEntity>(type.Name);
        }
    }
}