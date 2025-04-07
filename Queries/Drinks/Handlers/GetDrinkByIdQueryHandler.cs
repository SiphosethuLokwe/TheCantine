using MediatR;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Queries.Drinks.Handlers
{
    public class GetDrinkByIdQueryHandler : IRequestHandler<GetDrinkByIdQuery, Drink>
    {
        private readonly CantinaContext _context;

        public GetDrinkByIdQueryHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<Drink> Handle(GetDrinkByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Drinks.FindAsync(request.Id);
        }
    }
}
