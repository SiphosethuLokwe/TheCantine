using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Commands.Drinks;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes.Handlers
{
    public class UpdateDishHandler : IRequestHandler<UpdateDishCommand, CommandResponse<bool>>
    {
        private readonly CantinaContext _context;

        public UpdateDishHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<bool>> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(request.Id);
                if (dish == null)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Drink not found."
                    };

                }

                dish.Name = request.Name;
                dish.Description = request.Description;
                dish.Price = request.Price;
                dish.Image = request.Image;

                _context.Entry(dish).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Could not update"
                    };
                }
                return new CommandResponse<bool>
                {
                    Success = true,
                    Message = "Drink updated successfully"
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
