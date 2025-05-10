using Cantina.Application.Interfaces;
using Cantina.Domain.Entities;

public class DrinkService : IDrinkService
{
    private readonly IGenericRepository<Drink> _drinkRepository;

    public DrinkService(IGenericRepository<Drink> drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<IEnumerable<Drink>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        int skip = (page - 1) * pageSize;
        return await _drinkRepository.GetAllAsync(skip, pageSize, cancellationToken);       
    }

    public async Task<Drink?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
      
      return await _drinkRepository.GetByIdAsync(id, cancellationToken);          
    }

    public async Task CreateAsync(Drink drink, CancellationToken cancellationToken)
    {
       
       await _drinkRepository.AddAsync(drink, cancellationToken);       
    }

    public async Task UpdateAsync(Drink drink, CancellationToken cancellationToken)
    {
       
       await _drinkRepository.UpdateAsync(drink, cancellationToken);   
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {      
       await _drinkRepository.DeleteAsync(id, cancellationToken);             
    }

    public async Task<Drink?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        
       return await _drinkRepository.GetFirstOrDefaultAsync(d => d.Name == name, cancellationToken);
           
    }
}
