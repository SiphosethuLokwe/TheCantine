using Cantina.Domain.Entities;
using MediatR;

namespace CantinaAPI.Queries.Drinks
{
    public class GetDrinksQuery : IRequest<IEnumerable<Drink>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
