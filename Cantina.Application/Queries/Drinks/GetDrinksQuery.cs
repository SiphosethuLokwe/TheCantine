using Cantina.Domain.Entities;
using MediatR;

namespace CantinaAPI.Queries.Drinks
{
    public class GetDrinksQuery : IRequest<IEnumerable<Drink>>
    {
    }
}
