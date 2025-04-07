using MediatR;
using TheCantine.Models;

namespace TheCantine.Queries.Drinks
{
    public class GetDrinkByIdQuery : IRequest<Drink>
    {
        public int Id { get; set; }
    }
}
