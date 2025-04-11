using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class UpdateDrinkCommandHandler : IRequestHandler<UpdateDrinkCommand, CommandResponse<bool>>
    {
        private readonly CantinaContext _context;

        public UpdateDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<bool>> Handle(UpdateDrinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var drink = await _context.Drinks.FindAsync(request.Id);
                if (drink == null)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Drink not found."
                    };

                }

                drink.Name = request.Name;
                drink.Description = request.Description;
                drink.Price = request.Price;
                drink.Image = request.Image;

                _context.Entry(drink).State = EntityState.Modified;
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
