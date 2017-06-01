using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dasomus.MongoRepository
{
    /// <summary>
    /// Mongo repository
    /// </summary>
    /// <typeparam name="T">Collection type.</typeparam>
    /// <typeparam name="TKey">Type for Id of entity.</typeparam>
    public class MongoRepository<T, TKey> : IMongoRepository<T, TKey>
        where T : IMongoEntity<TKey>
    {
        private IMongoCollection<T> _collection;
        private IMongoContext _mongoContext;
        
        public MongoRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;

            if (_mongoContext == null)
            {
                throw new Exception("Mongo context object is null.");
            }

            if (_mongoContext.Url != null)
            {
                _collection = Util<TKey>.GetCollectionFromUrl<T>(_mongoContext.Url);
            }
            else if (!String.IsNullOrWhiteSpace(_mongoContext.ConnectionString))
            {
                _collection = Util<TKey>.GetCollectionFromConnectionString<T>(_mongoContext.ConnectionString);
            }
            else
            {
                throw new Exception("Mongo context has not been configured properly.");
            }
        }

        public IMongoCollection<T> Collection
        {
            get { return _collection; }
        }

        public string CollectionName
        {
            get { return _collection.CollectionNamespace.CollectionName; }
        }

        public T GetById(TKey id)
        {
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(id));
            return _collection.Find(filter).FirstOrDefault();
        }

        public Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(id));
            return _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public T Add(T entity)
        {
            _collection.InsertOne(entity);

            return entity;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);

            return entity;
        }

        public void Add(IEnumerable<T> entities)
        {
            _collection.InsertMany(entities);
        }

        public Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _collection.InsertManyAsync(entities, null, cancellationToken);
        }

        public T Update(T entity)
        {
            var idFilter = Builders<T>.Filter.Eq(e => e.Id, entity.Id); //Find entity with same Id

            var result = _collection.ReplaceOne(idFilter, entity, new UpdateOptions { IsUpsert = true });

            return entity;
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var idFilter = Builders<T>.Filter.Eq(e => e.Id, entity.Id); //Find entity with same Id

            var result = await _collection.ReplaceOneAsync(idFilter, entity, new UpdateOptions { IsUpsert = true }, cancellationToken);

            return entity;
        }

        public void Delete(TKey id)
        {
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(id));
            _collection.DeleteOne(filter);
        }

        public Task DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(id));
            return _collection.DeleteOneAsync(filter, cancellationToken);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            foreach (T entity in this._collection.AsQueryable<T>().Where(predicate))
            {
                Delete(entity.Id);
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (T entity in this._collection.AsQueryable<T>().Where(predicate))
            {
                await DeleteAsync(entity.Id, cancellationToken);
            }
        }

        public void DeleteAll()
        {
            _collection.DeleteMany(new BsonDocument());
        }

        public Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _collection.DeleteManyAsync(new BsonDocument(), cancellationToken);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _collection.AsQueryable<T>().Any(predicate);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _collection.AsQueryable<T>().AnyAsync(predicate, cancellationToken);
        }

        #region IQueryable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.AsQueryable<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.AsQueryable<T>().GetEnumerator();
        }

        public QueryableExecutionModel GetExecutionModel()
        {
            return _collection.AsQueryable<T>().GetExecutionModel();
        }

        public IAsyncCursor<T> ToCursor(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _collection.AsQueryable<T>().ToCursor(cancellationToken);
        }

        public Task<IAsyncCursor<T>> ToCursorAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _collection.AsQueryable<T>().ToCursorAsync(cancellationToken);
        }

        public Type ElementType
        {
            get { return _collection.AsQueryable<T>().ElementType; }
        }

        public Expression Expression
        {
            get { return _collection.AsQueryable<T>().Expression; }
        }
        
        public IQueryProvider Provider
        {
            get { return _collection.AsQueryable<T>().Provider; }
        }

        #endregion
    }

    public class MongoRepository<T> : MongoRepository<T, ObjectId>, IMongoRepository<T>
        where T : IMongoEntity<ObjectId>
    {
        public MongoRepository(IMongoContext mongoContext)
            : base(mongoContext) { }
    }
}
