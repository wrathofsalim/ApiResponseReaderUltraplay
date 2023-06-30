using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Entities;
using UP.Core.Models;

namespace UP.Services.Services;

public class SportService : ISportService
{
    private readonly IUnitOfWork unitOfWork;

    public SportService(IUnitOfWork unitOfWork)
        => this.unitOfWork = unitOfWork;


    public async Task<SportModel> CreateAsync(SportModel model)
    {
        var sportEntity = new SportEntity
        {
            Id = model.Id,
            Name = model.Name
        };

        await unitOfWork.Sports.CreateAsync(sportEntity);
        await unitOfWork.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<SportModel>> GetAllAsync()
    {
        var sportEntities = await unitOfWork.Sports.GetAllAsync();
        return sportEntities.Select(a => new SportModel
        {
            Id = a.Id,
            Name = a.Name,
            IsActive = a.IsActive,
        });
    }

    public async Task<SportModel> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.Sports.GetByIdAsync(id);

        if (entity != null)
        {
            return new SportModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IsActive = entity.IsActive,
            };
        }
     
        return null;
    }

    public async Task<SportModel> UpdateAsync(SportModel model)
    {
        return model;
    }
}
