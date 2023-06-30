using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class EventRepository : GenericRepository<EventEntity>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }
}
