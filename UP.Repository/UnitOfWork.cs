using UP.Core.Contracts;
using UP.Core.Contracts.Repositories;
using UP.DataLayer;

namespace UP.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext appDbContext;

    public UnitOfWork(
        AppDbContext appDbContext,
        IBetRepository bets,
        IEventRepository events,
        IMatchRepository matches,
        IOddRepository odds,
        ISportRepository sports,
        IMainRepository main
        )
    {
        this.appDbContext = appDbContext;
        Bets = bets;
        Events = events;
        Matches = matches;
        Odds = odds;
        Sports = sports;
        Main = main;
    }

    public IBetRepository Bets { get; }

    public IEventRepository Events { get; }

    public IMatchRepository Matches { get; }

    public IOddRepository Odds { get; }

    public ISportRepository Sports { get; }

    public IMainRepository Main { get; }

    public async Task SaveChangesAsync()
    {
        try
        {
            await appDbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            new Exception(ex.Message, ex);
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            appDbContext.Dispose();
        }
    }
}
