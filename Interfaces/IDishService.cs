using TheCantine.Models;

namespace TheCantine.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<Dish>> GetDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task<Dish> CreateDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
    }
}
