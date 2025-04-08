using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;
using TheCantine.Queries.Dishes;

namespace TheCantine.Queries.Dishes.Handlers
{
    public class GetDishesQueryHandler : IRequestHandler<GetDishesQuery, IEnumerable<Dish>>
    {
        private readonly CantinaContext _context;

        public GetDishesQueryHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> Handle(GetDishesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Dishes.ToListAsync(cancellationToken);
        }
    }
}
