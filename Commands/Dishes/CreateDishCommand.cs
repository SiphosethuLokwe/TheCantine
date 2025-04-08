using MediatR;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes
{
    public class CreateDishCommand : IRequest<CommandResponse<Dish>>
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
