using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YouTubeApp.Core.Models;

namespace YouTubeApp.Data
{
    public class MongoDBRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        protected IMongoCollection<T> _collection;
        public IMongoCollection<T> Collection
        {
            get
            {
                return _collection;
            }
        }

        protected IMongoDatabase _database;
        public IMongoDatabase Database
        {
            get
            {
                return _database;
            }
        }

        #endregion

        #region Ctor

        
        public MongoDBRepository(IOptions<DataSettings> options)
        {
            string connectionString = options.Value.ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                var client = new MongoClient(connectionString);
                var databaseName = options.Value.DatabaseName; //new MongoUrl(connectionString).DatabaseName;
                _database = client.GetDatabase(databaseName);
                _collection = _database.GetCollection<T>(typeof(T).Name);
            }
        }
        public MongoDBRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        //TODO: REMOVER
        //public MongoDBRepository(IMongoClient client)
        //{
        //    string connectionString = DataSettingsHelper.ConnectionString();
        //    var databaseName = new MongoUrl(connectionString).DatabaseName;
        //    _database = client.GetDatabase(databaseName);
        //    _collection = _database.GetCollection<T>(typeof(T).Name);
        //}

        public MongoDBRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        #endregion

        #region Methods

        public virtual T GetById(string id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefault();
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
        public virtual T Insert(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }


        public virtual void Insert(IEnumerable<T> entities)
        {
            _collection.InsertMany(entities);
        }

        public virtual async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
            return entities;
        }


        public virtual T Update(T entity)
        {
            _collection.ReplaceOne(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
            return entity;

        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
            return entity;
        }


        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Update(entity);
            }
        }

        public virtual async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
            return entities;
        }

        public virtual void Delete(T entity)
        {
            _collection.FindOneAndDelete(e => e.Id == entity.Id);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
            return entity;
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _collection.FindOneAndDeleteAsync(e => e.Id == entity.Id);
            }
        }

        public virtual async Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await DeleteAsync(entity);
            }
            return entities;
        }


        #endregion


        #region Methods

        public virtual bool Any()
        {
            return _collection.AsQueryable().Any();
        }

        public virtual bool Any(Expression<Func<T, bool>> where)
        {
            return _collection.Find(where).Any();
        }

        public virtual async Task<bool> AnyAsync()
        {
            return await _collection.AsQueryable().AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _collection.Find(where).AnyAsync();
        }

        public virtual long Count()
        {
            return _collection.CountDocuments(new BsonDocument());
        }

        public virtual long Count(Expression<Func<T, bool>> where)
        {
            return _collection.CountDocuments(where);
        }

        public virtual async Task<long> CountAsync()
        {
            return await _collection.CountDocumentsAsync(new BsonDocument());
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await _collection.CountDocumentsAsync(where);
        }

        public virtual async Task<T> GetSingleAsync()
        {
            return await Table.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Properties

        public virtual IMongoQueryable<T> Table
        {
            get { return _collection.AsQueryable(); }
        }

        public virtual IList<T> FindByFilterDefinition(FilterDefinition<T> query)
        {
            return _collection.Find(query).ToList();
        }


        #endregion
    }
}
