using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TheCantine.Data;
using TheCantine.Interfaces;
using TheCantine.Models;

namespace TheCantine.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly CantinaContext _context;
        private readonly ILogger<DrinkService> _logger;

        public DrinkService(CantinaContext context, ILogger<DrinkService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Drink>> GetDrinksAsync()
        {
            return await _context.Drinks.ToListAsync();
        }

        public async Task<Drink> GetDrinkByIdAsync(int id)
        {
            return await _context.Drinks.FindAsync(id);
        }

        public async Task<Drink> CreateDrinkAsync(Drink drink)
        {
            ValidateDrink(drink);

            try
            {
                _context.Drinks.Add(drink);
                await _context.SaveChangesAsync();
                return drink;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the drink.");
                throw new ApplicationException("An error occurred while creating the drink.", ex);
            }
        }

        public async Task UpdateDrinkAsync(Drink drink)
        {
            ValidateDrink(drink);

            try
            {
                _context.Entry(drink).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DrinkExists(drink.Id))
                {
                    throw new KeyNotFoundException("Drink not found.");
                }
                else
                {
                    _logger.LogError(ex, "An error occurred while updating the drink.");
                    throw new ApplicationException("An error occurred while updating the drink.", ex);
                }
            }
        }
         

        public async Task DeleteDrinkAsync(int id)
        {
            try
            {
                var drink = await _context.Drinks.FindAsync(id);
                if (drink != null)
                {
                    _context.Drinks.Remove(drink);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Drink not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the drink.");
                throw new ApplicationException("An error occurred while deleting the drink.", ex);
            }
        }

        private void ValidateDrink(Drink drink)
        {
            var validationContext = new ValidationContext(drink);
            Validator.ValidateObject(drink, validationContext, validateAllProperties: true);
        }

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
