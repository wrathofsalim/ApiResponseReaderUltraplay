using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Entities;
using UP.Core.Models;

namespace UP.Services.Services;

public class OddService : IOddService
{
    private readonly IUnitOfWork unitOfWork;

    public OddService(IUnitOfWork unitOfWork)
    => this.unitOfWork = unitOfWork;

    public async Task<OddModel> CreateAsync(OddModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var existingOdd = await unitOfWork.Odds.GetByIdAsync(model.Id);
        if (existingOdd != null)
        {
            throw new InvalidOperationException("Odd with the specified ID already exists.");
        }

        var oddEntity = new OddEntity
        {
            Id = model.Id,
            Name = model.Name,
            Value = model.Value,
            BetEntityId = model.BetModelId,
            SpecialBetValue = model.SpecialBetValue,
            IsActive = model.IsActive,
        };

        await unitOfWork.Odds.CreateAsync(oddEntity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<OddModel>> GetAllAsync()
    {
        var sportEntities = await unitOfWork.Odds.GetAllAsync();
        return sportEntities.Select(a => new OddModel
        {
            Id = a.Id,
            Name = a.Name,
            Value = a.Value,
            BetModelId = a.BetEntityId,
            SpecialBetValue = a.SpecialBetValue,
            IsActive = a.IsActive,
        });
    }

    public async Task<OddModel> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.Odds.GetByIdAsync(id);

        if (entity != null)
        {
            return new OddModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                BetModelId = entity.BetEntityId,
                Value = entity.Value,
                SpecialBetValue = entity.SpecialBetValue,
                IsActive = entity.IsActive,
            };
        }
     
        return null;
    }

    public async Task<OddModel> UpdateAsync(OddModel model)
    {
        OddEntity entity = await unitOfWork.Odds.GetByIdAsync(model.Id);

        entity.Id = model.Id;
        entity.Name = model.Name;
        entity.Value = model.Value;
        entity.BetEntityId = model.BetModelId;
        entity.SpecialBetValue = model.SpecialBetValue;
        entity.IsActive = model.IsActive;

        unitOfWork.Odds.Update(entity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }
}
