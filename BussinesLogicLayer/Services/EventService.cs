using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Entities;
using UP.Core.Models;

namespace UP.Services.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork unitOfWork;

    public EventService(IUnitOfWork unitOfWork)
    => this.unitOfWork = unitOfWork;

    public async Task<EventModel> CreateAsync(EventModel model)
    {    
        var eventEntity = new EventEntity
        {
            Id = model.Id,
            Name = model.Name,
            IsLive = model.IsLive,
            CategoryId = model.CategoryId,
            SportEntityId = model.SportModelId,
            IsActive = model.IsActive,
        };

        await unitOfWork.Events.CreateAsync(eventEntity);

        await unitOfWork.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<EventModel>> GetAllAsync()
    {
        var eventsEntities = await unitOfWork.Events.GetAllAsync();

        return eventsEntities.Select(a => new EventModel
        {
            Id = a.Id,
            Name = a.Name,
            IsLive = a.IsLive,
            CategoryId = a.CategoryId,
            SportModelId = a.SportEntityId,
            IsActive = a.IsActive
        });
    }

    public async Task<EventModel> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.Events.GetByIdAsync(id);

        if (entity != null)
        {
            return new EventModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IsLive = entity.IsLive,
                CategoryId = entity.CategoryId,
                SportModelId = entity.SportEntityId,
                IsActive = entity.IsActive
            };
        }
     
        return null;
    }

    public async Task<EventModel> UpdateAsync(EventModel model)
    {
        EventEntity entity = await unitOfWork.Events.GetByIdAsync(model.Id);
        entity.Name = model.Name;
        entity.IsLive = model.IsLive;
        entity.IsActive = model.IsActive;
        entity.CategoryId = model.CategoryId;
        entity.SportEntityId = model.SportModelId;

        unitOfWork.Events.Update(entity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }
}
