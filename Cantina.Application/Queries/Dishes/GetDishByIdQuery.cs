using Cantina.Domain.Entities;
using MediatR;

namespace CantinaAPI.Queries.Dishes
{
    public class GetDishByIdQuery : IRequest<Dish>
    {
        public int Id { get; set; }
    }
}
