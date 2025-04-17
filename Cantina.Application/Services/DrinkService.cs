using Cantina.Application.Interfaces;
using Cantina.Domain.Entities;

public class DrinkService : IDrinkService
{
    private readonly IGenericRepository<Drink> _drinkRepository;

    public DrinkService(IGenericRepository<Drink> drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<IEnumerable<Drink>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _drinkRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving drinks.", ex);
        }
    }

    public async Task<Drink?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _drinkRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error getting drink with ID {id}.", ex);
        }
    }

    public async Task CreateAsync(Drink drink, CancellationToken cancellationToken)
    {
        try
        {
            await _drinkRepository.AddAsync(drink, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to create drink.", ex);
        }
    }

    public async Task UpdateAsync(Drink drink, CancellationToken cancellationToken)
    {
        try
        {
            await _drinkRepository.UpdateAsync(drink, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to update drink.", ex);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _drinkRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to delete drink with ID {id}.", ex);
        }
    }

    public async Task<Drink?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        try
        {
            return await _drinkRepository.GetFirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error retrieving drink by name '{name}'.", ex);
        }
    }
}
