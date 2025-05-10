using Cantina.Domain.Entities;
using MediatR;

namespace Cantina.Application.Queries.Dishes
{

    public class GetDishesQuery : IRequest<IEnumerable<Dish>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
