namespace UP.Core.Contracts.Services;

public interface IGenericService<TModel> where TModel : class
{
    Task<TModel> GetByIdAsync(int id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> CreateAsync(TModel model);

    Task<TModel> UpdateAsync(TModel model);
}