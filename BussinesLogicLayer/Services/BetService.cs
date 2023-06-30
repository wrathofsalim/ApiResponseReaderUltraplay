using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Entities;
using UP.Core.Models;

namespace UP.Services.Services;

public class BetService : IBetService
{
    private readonly IUnitOfWork unitOfWork;

    public BetService(IUnitOfWork unitOfWork)
    => this.unitOfWork = unitOfWork;

    public async Task<BetModel> CreateAsync(BetModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var existingBets = await unitOfWork.Bets.GetByIdAsync(model.Id);
        if (existingBets != null)
        {
            throw new InvalidOperationException("Bet with the specified ID already exists.");
        }

        var betEntity = new BetEntity
        {
            Id = model.Id,
            Name = model.Name,
            IsLive = model.IsLive,
            MatchEntityId = model.MatchModelId,
            IsActive = model.IsActive
        };

        await unitOfWork.Bets.CreateAsync(betEntity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<BetModel>> GetAllAsync()
    {
        var betEntities = await unitOfWork.Bets.GetAllAsync();


        return betEntities.Select(a => new BetModel
        {
            Id = a.Id,
            Name = a.Name,
            IsLive = a.IsLive,
            MatchModelId = a.MatchEntityId,
            IsActive = a.IsActive
        });
    }

    public async Task<BetModel> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.Bets.GetByIdAsync(id);

        if (entity != null)
        {
            return new BetModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IsLive = entity.IsLive,
                MatchModelId = entity.MatchEntityId,
                IsActive = entity.IsActive
            };
        }
        
        return null;
    }

    public async Task<BetModel> UpdateAsync(BetModel model)
    {
        BetEntity entity = await unitOfWork.Bets.GetByIdAsync(model.Id);
        entity.Name = model.Name;
        entity.IsLive = model.IsLive;
        entity.MatchEntityId = model.MatchModelId;
        entity.IsActive = model.IsActive;

        unitOfWork.Bets.Update(entity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }
}
