using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Common
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly eVote360ProContext _context;

        public GenericRepository(eVote360ProContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity?> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.IsActive = false;
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                    _context.Set<TEntity>().Update(entity);
                }
                else
                {
                    _context.Set<TEntity>().Remove(entity);
                }

                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAllQuery()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            var existing = await _context.Set<TEntity>().FindAsync(id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existing;
        }

        public virtual async Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return await query.ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAllQueryWithInclude(List<string> properties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return query;
        }
    }
}
