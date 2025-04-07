using TheCantine.Models;

namespace TheCantine.Interfaces
{
    public interface IDrinkService
    {
        Task<IEnumerable<Drink>> GetDrinksAsync();
        Task<Drink> GetDrinkByIdAsync(int id);
        Task<Drink> CreateDrinkAsync(Drink drink);
        Task UpdateDrinkAsync(Drink drink);
        Task DeleteDrinkAsync(int id);
    }
}
