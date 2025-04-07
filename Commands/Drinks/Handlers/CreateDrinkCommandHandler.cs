using MediatR;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class CreateDrinkCommandHandler : IRequestHandler<CreateDrinkCommand, Drink>
    {
        private readonly CantinaContext _context;

        public CreateDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<Drink> Handle(CreateDrinkCommand request, CancellationToken cancellationToken)
        {
            var drink = new Drink
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Image = request.Image
            };

            _context.Drinks.Add(drink);
            await _context.SaveChangesAsync(cancellationToken);

            return drink;
        }
    }
}
