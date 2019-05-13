using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDbGenericRepository.Abstractions
{
    public interface IRepository
    {
        Task<TEntity> Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : IEntity;
        Task<List<TEntity>> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : IEntity;
        Task Add<TEntity>(TEntity item) where TEntity : IEntity;
        Task<long> Update<TEntity>(Expression<Func<TEntity, bool>> expression, TEntity item) where TEntity : IEntity;
        Task<long> Delete<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : IEntity;
        Task DeleteAll();
    }
}