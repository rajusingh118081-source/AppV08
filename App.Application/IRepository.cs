using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Application
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        // Create
        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities);

        // Update
        Task UpdateAsync(TEntity entity);

        // Delete
        Task DeleteAsync(int id);

        // Read
        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity?> FindUniqueAsync(
            string uniqueNumber);

        Task<IEnumerable<TEntity>> GetAllAsync();

        // Query
        IQueryable<TEntity> GetAllQueryable();

        IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate);
    }



}
