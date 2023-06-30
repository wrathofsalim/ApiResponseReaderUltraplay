using UP.Core.Models;

namespace UP.Core.Contracts.Services;

public interface IMainService
{
    Task<IEnumerable<MatchModel>> GetAllMatches();

    Task<SingleMatchModel> GetSingleMatchByIdAsync(int id);
}