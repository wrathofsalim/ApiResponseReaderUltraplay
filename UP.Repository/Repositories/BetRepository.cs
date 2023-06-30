using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class BetRepository : GenericRepository<BetEntity>, IBetRepository
{
    public BetRepository(AppDbContext context) : base(context) { }
}
