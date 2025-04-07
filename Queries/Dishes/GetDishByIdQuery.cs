using MediatR;
using TheCantine.Models;

namespace TheCantine.Queries.Dishes
{
    public class GetDishByIdQuery : IRequest<Dish>
    {
        public int Id { get; set; }
    }
}
