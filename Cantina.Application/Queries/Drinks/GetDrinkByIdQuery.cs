using Cantina.Domain.Entities;
using MediatR;

namespace CantinaAPI.Queries.Drinks
{
    public class GetDrinkByIdQuery : IRequest<Drink>
    {
        public int Id { get; set; }
    }
}
