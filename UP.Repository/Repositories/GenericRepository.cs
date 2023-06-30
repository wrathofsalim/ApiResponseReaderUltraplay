using Microsoft.EntityFrameworkCore;
using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext context;

    private readonly DbSet<TEntity> entities;

    public GenericRepository(AppDbContext context)
    {
        this.context = context;
        entities = context.Set<TEntity>();
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
        => await entities.FirstOrDefaultAsync(entity => entity.Id == id);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await entities.Where(e => e.IsActive == true).ToListAsync();

    public virtual async Task CreateAsync(TEntity entity)
        => await entities.AddAsync(entity);

    public virtual void Update(TEntity entity)
        => context.Entry(entity).State = EntityState.Modified;
}
