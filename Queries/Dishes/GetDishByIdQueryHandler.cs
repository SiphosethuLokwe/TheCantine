using MediatR;
using TheCantine.Data;
using TheCantine.Models;
using TheCantine.Queries.Dishes;

namespace TheCantine.Commands.Dishes
{
    public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, Dish>
    {
        private readonly CantinaContext _context;

        public GetDishByIdQueryHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<Dish> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Dishes.FindAsync(request.Id).ConfigureAwait(false);
        }
    }
}
