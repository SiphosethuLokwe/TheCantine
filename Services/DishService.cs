using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TheCantine.Data;
using TheCantine.Interfaces;
using TheCantine.Models;

namespace TheCantine.Services
{
    public class DishService : IDishService
    {
        private readonly CantinaContext _context;
        private readonly ILogger<DishService> _logger;

        public DishService(CantinaContext context, ILogger<DishService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Dish>> GetDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task<Dish> CreateDishAsync(Dish dish)
        {
            ValidateDish(dish);

            try
            {
                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();
                return dish;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the dish.");
                throw new ApplicationException("An error occurred while creating the dish.", ex);
            }
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            ValidateDish(dish);

            try
            {
                _context.Entry(dish).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DishExists(dish.Id))
                {
                    throw new KeyNotFoundException("Dish not found.");
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while updating the dish.");
                    throw new ApplicationException("An error occurred while updating the dish.", ex);
                }
            }
        }

        public async Task DeleteDishAsync(int id)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(id);
                if (dish != null)
                {
                    _context.Dishes.Remove(dish);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Dish not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the dish.");
                throw new ApplicationException("An error occurred while deleting the dish.", ex);
            }
        }

        private void ValidateDish(Dish dish)
        {
            var validationContext = new ValidationContext(dish);
            Validator.ValidateObject(dish, validationContext, validateAllProperties: true);
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
