using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class OddRepository : GenericRepository<OddEntity>, IOddRepository
{
    public OddRepository(AppDbContext context) : base(context) { }
}
