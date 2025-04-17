using Cantina.Application.Interfaces;
using Cantina.Domain.Entities;

public class DishService : IDishService
{
    private readonly IGenericRepository<Dish> _dishRepository;

    public DishService(IGenericRepository<Dish> dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public async Task<IEnumerable<Dish>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _dishRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
           
            throw new ApplicationException("An error occurred while retrieving dishes.", ex);
        }
    }

    public async Task<Dish?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _dishRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error getting dish with ID {id}.", ex);
        }
    }

    public async Task CreateAsync(Dish dish, CancellationToken cancellationToken)
    {
        try
        {
            await _dishRepository.AddAsync(dish, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to create dish.", ex);
        }
    }

    public async Task UpdateAsync(Dish dish, CancellationToken cancellationToken)
    {
        try
        {
            await _dishRepository.UpdateAsync(dish, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to update dish.", ex);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _dishRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete dish with ID {id}.", ex);
        }
    }

    public async Task<Dish?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            return await _dishRepository.GetFirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error retrieving dish by name '{name}'.", ex);
        }
    }
}
