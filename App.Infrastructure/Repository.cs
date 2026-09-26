using AapRepository;
using App.Application;
using App.Domain;
using FastMember;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Linq.Expressions;


namespace App.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity>where TEntity : BaseEntity
    {
        protected readonly DB_Contexts _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DB_Contexts context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));

            _dbSet = _context.Set<TEntity>();
        }

        #region Create

        public virtual async Task<TEntity> AddAsync(
            TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            PrepareEntityForCreate(entity);

            await _dbSet.AddAsync(entity);

            return entity;
        }

        public virtual async Task AddRangeAsync(
            IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var entityList = entities.ToList();

            foreach (var entity in entityList)
            {
                PrepareEntityForCreate(entity);
            }

            await _dbSet.AddRangeAsync(entityList);
        }

        #endregion

        #region Update

        public virtual Task UpdateAsync(
            TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            PrepareEntityForUpdate(entity);

            _dbSet.Update(entity);

            return Task.CompletedTask;
        }

        #endregion

        #region Delete

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException(
                    $"{typeof(TEntity).Name} with ID {id} was not found.");
            }

            _dbSet.Remove(entity);
        }

        #endregion

        #region Read

        public virtual async Task<TEntity?> GetByIdAsync(
            int id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        public virtual async Task<TEntity?> FindUniqueAsync(
            string uniqueNumber)
        {
            if (string.IsNullOrWhiteSpace(uniqueNumber))
                return null;

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UniqueNumber == uniqueNumber);
        }

        public virtual async Task<IEnumerable<TEntity>>
            GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        #endregion

        #region Query

        public virtual IQueryable<TEntity>
            GetAllQueryable()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet
                .AsNoTracking()
                .Where(predicate);
        }

        public virtual async Task<TEntity?>
            SingleOrDefaultAsync(
                Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(predicate);
        }

        #endregion

        #region Audit

        protected virtual void PrepareEntityForCreate(
            TEntity entity)
        {
            entity.AddedDate = DateTime.UtcNow;

            PrepareEntityForUpdate(entity);
        }

        protected virtual void PrepareEntityForUpdate(
            TEntity entity)
        {
            entity.EditedDate = DateTime.UtcNow;
        }

        #endregion
    }


}
