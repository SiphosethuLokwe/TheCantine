using MediatR;
using TheCantine.Data;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class DeleteDrinkCommandHandler : IRequestHandler<DeleteDrinkCommand>
    {
        private readonly CantinaContext _context;

        public DeleteDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDrinkCommand request, CancellationToken cancellationToken)
        {
            var drink = await _context.Drinks.FindAsync(request.Id);
            if (drink == null)
            {
                throw new KeyNotFoundException("Drink not found.");
            }

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
