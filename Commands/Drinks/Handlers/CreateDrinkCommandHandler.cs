using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class CreateDrinkCommandHandler : IRequestHandler<CreateDrinkCommand, CommandResponse<Drink>>
    {
        private readonly CantinaContext _context;

        public CreateDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<Drink>> Handle(CreateDrinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDrink = await _context.Drinks
                                    .FirstOrDefaultAsync(d => d.Name == request.Name, cancellationToken);
                if (existingDrink != null)
                {
                    return new CommandResponse<Drink>
                    {
                        Success = false,
                        Message = "Drink already exists."
                    };
                }

                var Drink = new Drink
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = request.Image
                };


                _context.Drinks.Add(Drink);
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                {
                    return new CommandResponse<Drink>
                    {
                        Success = false,
                        Message = "Could not create Drink"
                    };
                }
                return new CommandResponse<Drink>
                {
                    Success = true,
                    Message = "Drink created successfully",
                    Data = Drink
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
