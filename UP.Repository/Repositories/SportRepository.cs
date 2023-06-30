using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class SportRepository : GenericRepository<SportEntity>, ISportRepository
{
    public SportRepository(AppDbContext context) : base(context) { }
}
