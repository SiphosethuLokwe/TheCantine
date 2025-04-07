using MediatR;
using TheCantine.Models;

namespace TheCantine.Queries.Drinks
{
    public class GetDrinksQuery : IRequest<IEnumerable<Drink>>
    {
    }
}
