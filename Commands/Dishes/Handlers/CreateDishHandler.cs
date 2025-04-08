using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes.Handlers
{
    public class CreateDishHandler : IRequestHandler<CreateDishCommand, CommandResponse<Dish>>
    {
        private readonly CantinaContext _context;

        public CreateDishHandler(CantinaContext context)
        {
            _context = context;
        }
        public async Task<CommandResponse<Dish>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDish = await _context.Dishes
                                    .FirstOrDefaultAsync(d => d.Name == request.Name, cancellationToken);
                if (existingDish != null)
                {
                    return new CommandResponse<Dish>
                    {
                        Success = false,
                        Message = "Dish already exists."
                    };
                }

                var dish = new Dish
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = request.Image
                };


                _context.Dishes.Add(dish);
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                {
                    return new CommandResponse<Dish>
                    {
                        Success = false,
                        Message = "Could not create dish"
                    };
                }
                return new CommandResponse<Dish>
                {
                    Success = true,
                    Message = "Dish created successfully",
                    Data = dish
                };

            }
            catch (Exception ex)
            {
                //logging will add later
                throw;
            }
             
        }
    }
}
