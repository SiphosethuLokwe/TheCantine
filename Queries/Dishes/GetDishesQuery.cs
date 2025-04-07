using MediatR;
using TheCantine.Models;

namespace TheCantine.Queries.Dishes
{


    public class GetDishesQuery : IRequest<IEnumerable<Dish>>
    {
    }
}
