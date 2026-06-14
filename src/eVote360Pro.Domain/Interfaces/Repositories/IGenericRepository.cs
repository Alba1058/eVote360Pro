namespace eVote360Pro.Core.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);
        Task DeleteAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllQuery();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> UpdateAsync(int id, TEntity entity);
        Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties);
        IQueryable<TEntity> GetAllQueryWithInclude(List<string> properties);
    }
}
