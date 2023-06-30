using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class MatchRepository : GenericRepository<MatchEntity>, IMatchRepository
{
    public MatchRepository(AppDbContext context) : base(context) { }
}
