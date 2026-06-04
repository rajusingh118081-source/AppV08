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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DB_Contexts _context;
        protected readonly int _currentUserId;

        public Repository(DB_Contexts context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        //public Repository(DB_Contexts context, IHttpContextAccessor httpContextAccessor)
        //{
        //    _context = context ?? throw new ArgumentNullException(nameof(context));

        //    // Example: You might extract user ID from claims
        //    _currentUserId = int.TryParse(httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value, out var userId)
        //        ? userId
        //        : 0;
        //}
        #region CRUD Operations

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            PrepareEntityForCreate(entity);
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            PrepareEntityForUpdate(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                throw new Exception("Entity not found");

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
        }

        public virtual async Task<TEntity?> FindUniqueAsync(string uniqueNumber)
        {
            return await _context.Set<TEntity>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.UniqueNumber == uniqueNumber);
        }

        #endregion

        #region Querying

        public virtual IQueryable<TEntity> GetAllQueryable()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<IQueryable<TEntity>> GetAllQueryableAsync()
        {
            var data = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            return data.AsQueryable();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public virtual TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().SingleOrDefault(predicate);
        }

        #endregion

        #region Utility

        protected virtual void PrepareEntityForCreate(TEntity entity)
        {
            entity.AddedDate = DateTime.UtcNow;
            entity.RefAddedByID = _currentUserId;
            PrepareEntityForUpdate(entity);
        }

        protected virtual void PrepareEntityForUpdate(TEntity entity)
        {
            entity.EditedDate = DateTime.UtcNow;
            entity.RefEditedByID = _currentUserId;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Optional / Legacy Support

        public virtual DataTable GetAllDataTable()
        {
            var data = _context.Set<TEntity>().ToList();
            using var reader = ObjectReader.Create(data);
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual IEnumerable<T> ReadActive<T>() where T : BaseEntity
        {
            var all = _context.Set<T>().AsNoTracking().ToList();

            var active = all
                .Where(t => !t.IsInactive)
                .OrderBy(t => t.Category)
                .ThenBy(t => t.Description);

            var inactive = all
                .Where(t => t.IsInactive)
                .OrderBy(t => t.Description)
                .Select(c =>
                {
                    c.Category = "Ξ Inactives Ξ";
                    return c;
                });

            return active.Concat(inactive).ToList();
        }

        #endregion
    }

}
