using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Entities;
using UP.Core.Models;

namespace UP.Services.Services;

public class MatchService : IMatchService
{
    private readonly IUnitOfWork unitOfWork;

    public MatchService(IUnitOfWork unitOfWork)
    => this.unitOfWork = unitOfWork;

    public async Task<MatchModel> CreateAsync(MatchModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var existingOdd = await unitOfWork.Matches.GetByIdAsync(model.Id);
        if (existingOdd != null)
        {
            throw new InvalidOperationException("Matches with the specified ID already exists.");
        }

        var matchEntity = new MatchEntity
        {
            Id = model.Id,
            Name = model.Name,
            MatchType = model.MatchType,
            StartDate = model.StartDate,
            EventEntityId = model.EventModelId,
            IsActive = model.IsActive,
        };

        await unitOfWork.Matches.CreateAsync(matchEntity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<MatchModel>> GetAllAsync()
    {
        var matchEntities = await unitOfWork.Matches.GetAllAsync();
        return matchEntities.Select(a => new MatchModel
        {
            Id = a.Id,
            Name = a.Name,
            MatchType = a.MatchType,
            StartDate = a.StartDate,
            EventModelId = a.EventEntityId,
            IsActive = a.IsActive,
        });
    }

    public async Task<MatchModel> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.Matches.GetByIdAsync(id);

        if (entity != null)
        {
            return new MatchModel
            {
                Id = entity.Id,
                Name = entity.Name,
                EventModelId = entity.EventEntityId,
                StartDate = entity.StartDate,
                MatchType = entity.MatchType,
                IsActive = entity.IsActive,
            };
        }
        
        return null;
    }

    public async Task<MatchModel> UpdateAsync(MatchModel model)
    {
        MatchEntity entity = await unitOfWork.Matches.GetByIdAsync(model.Id);
        entity.Name = model.Name;
        entity.MatchType = model.MatchType;
        entity.StartDate = model.StartDate;
        entity.EventEntityId = model.EventModelId;
        entity.IsActive = model.IsActive;

        unitOfWork.Matches.Update(entity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }
}
