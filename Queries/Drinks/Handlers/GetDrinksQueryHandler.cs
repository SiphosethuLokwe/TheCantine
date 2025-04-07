using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Queries.Drinks.Handlers
{
    public class GetDrinksQueryHandler : IRequestHandler<GetDrinksQuery, IEnumerable<Drink>>
    {
        private readonly CantinaContext _context;

        public GetDrinksQueryHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drink>> Handle(GetDrinksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Drinks.ToListAsync();
        }
    }
}
