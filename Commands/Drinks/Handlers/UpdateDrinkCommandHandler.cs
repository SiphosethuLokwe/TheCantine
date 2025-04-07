using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class UpdateDrinkCommandHandler : IRequestHandler<UpdateDrinkCommand>
    {
        private readonly CantinaContext _context;

        public UpdateDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDrinkCommand request, CancellationToken cancellationToken)
        {
            var drink = await _context.Drinks.FindAsync(request.Id);
            if (drink == null)
            {
                throw new KeyNotFoundException("Drink not found.");
            }

            drink.Name = request.Name;
            drink.Description = request.Description;
            drink.Price = request.Price;
            drink.Image = request.Image;

            _context.Entry(drink).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
