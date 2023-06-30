using Microsoft.EntityFrameworkCore;
using UP.Core.Contracts.Repositories;
using UP.Core.Entities;
using UP.DataLayer;

namespace UP.Repository.Repositories;

public class MainRepository : IMainRepository
{
    protected readonly AppDbContext context;

    public MainRepository(AppDbContext context)
    => this.context = context;

    public async Task<IEnumerable<MatchEntity>> GetAllMatchesAsync()
    => await context.Matches.Where(b => b.IsActive).ToListAsync();

    public async Task<IEnumerable<BetEntity>> GetAllBetsByMatchIdAsync(int id)
    => await context.Bets
        .Include(o => o.Odds)
        .Where(o => o.MatchEntityId == id)
        .Where(m => m.Name == "Match Winner" || m.Name == "Map Advantage" || m.Name == "Total Maps Played")
        .Where(m => m.Odds.Any(o => o.SpecialBetValue == null) || m.Odds.GroupBy(o => o.SpecialBetValue).Select(g => g.Key).Count() == 1)
        .ToListAsync();

    public async Task<IEnumerable<OddEntity>> GetAllOddsByBetIdAsync(int id)
      => await context.Odds
            .Where(b => b.BetEntityId == id && b.IsActive)
            .ToListAsync();

    public async Task<MatchEntity> GetSingleMatchByIdAsync(int matchId)
    => await context.Matches
            .Include(m => m.Bets)
            .ThenInclude(o => o.Odds)
            .FirstOrDefaultAsync(m => m.Id == matchId);
}
