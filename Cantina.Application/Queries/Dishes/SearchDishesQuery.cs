using Cantina.Domain.Entities;
using MediatR;


namespace Cantina.Application.Queries.Dishes
{
    public class SearchDishesQuery : IRequest<IEnumerable<Dish>>
    {
        public string SearchTerm { get; set; }
    }

}
