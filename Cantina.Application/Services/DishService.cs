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
        
       return await _dishRepository.GetAllAsync(cancellationToken);    
       
    }

    public async Task<Dish?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {       
        return await _dishRepository.GetByIdAsync(id, cancellationToken);        
    }

    public async Task CreateAsync(Dish dish, CancellationToken cancellationToken)
    {
       
      await _dishRepository.AddAsync(dish, cancellationToken);
            
    }

    public async Task UpdateAsync(Dish dish, CancellationToken cancellationToken)
    {
       
        await _dishRepository.UpdateAsync(dish, cancellationToken);
           
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
       
       await _dishRepository.DeleteAsync(id, cancellationToken);
      
    }

    public async Task<Dish?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {      
        return await _dishRepository.GetFirstOrDefaultAsync(d => d.Name == name, cancellationToken);             
    }
}
