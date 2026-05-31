using App.Domain;
using App.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();

        //read side (could be in separate Readonly Generic Repository)
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> FindUniqueAsync(string uniqueNumber);

        // This method was not in the videos, but I thought it would be useful to add.
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAllQueryable();
        Task<IQueryable<TEntity>> GetAllQueryableAsync();
        IEnumerable<T> ReadActive<T>() where T : BaseEntity;
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
