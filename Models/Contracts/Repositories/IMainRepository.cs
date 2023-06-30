using UP.Core.Entities;

namespace UP.Core.Contracts.Repositories;

public interface IMainRepository
{
    Task<IEnumerable<MatchEntity>> GetAllMatchesAsync();

    Task<IEnumerable<BetEntity>> GetAllBetsByMatchIdAsync(int id);

    Task<IEnumerable<OddEntity>> GetAllOddsByBetIdAsync(int id);

    Task<MatchEntity> GetSingleMatchByIdAsync(int id);
}