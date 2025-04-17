using Cantina.Domain.Entities;

public interface IDrinkService
{
    Task CreateAsync(Drink drink, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Drink>> GetAllAsync(CancellationToken cancellationToken);
    Task<Drink?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Drink?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task UpdateAsync(Drink drink, CancellationToken cancellationToken);
}