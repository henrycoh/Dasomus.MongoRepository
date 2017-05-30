using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dasomus.MongoRepository
{
    public interface IMongoRepository<T, TKey> : IQueryable<T>
        where T : IMongoEntity<TKey>
    {
        IMongoCollection<T> Collection { get; }

        T GetById(TKey id);

        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

        T Add(T entity);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        void Add(IEnumerable<T> entities);

        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        T Update(T entity);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(TKey id);

        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        
        void Delete(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        void DeleteAll();

        Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        bool Exists(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
    
    public interface IMongoRepository<T> : IQueryable<T>, IMongoRepository<T, ObjectId>
        where T : IMongoEntity<ObjectId>
    { }
}
