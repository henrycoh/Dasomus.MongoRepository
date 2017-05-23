using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dasomus.MongoRepository
{
    public interface IMongoRepository<T, TKey> : IQueryable<T>
        where T : IMongoEntity<TKey>
    {
        IMongoCollection<T> Collection { get; }

        T GetById(TKey id);

        T Add(T entity);

        void Add(IEnumerable<T> entities);

        T Update(T entity);

        void Delete(TKey id);
        
        void Delete(Expression<Func<T, bool>> predicate);

        void DeleteAll();

        bool Exists(Expression<Func<T, bool>> predicate);
    }
    
    public interface IMongoRepository<T> : IQueryable<T>, IMongoRepository<T, ObjectId>
        where T : IMongoEntity<ObjectId>
    { }
}
