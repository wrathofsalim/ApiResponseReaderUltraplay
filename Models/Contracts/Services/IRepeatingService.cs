namespace UP.Core.Contracts.Services;

public interface IRepeatingService : IDisposable
{
    public Task CallLink();
}