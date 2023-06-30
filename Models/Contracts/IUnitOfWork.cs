using UP.Core.Contracts.Repositories;

namespace UP.Core.Contracts;

public interface IUnitOfWork
{
    IBetRepository Bets { get; }

    IEventRepository Events { get; }

    IMatchRepository Matches { get; }

    IOddRepository Odds { get; }

    ISportRepository Sports { get; }

    IMainRepository Main { get; }

    Task SaveChangesAsync();

    void Dispose();
}