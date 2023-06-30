namespace UP.Core.Contracts.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task CreateAsync(TEntity entity);

    void Update(TEntity entity);
}