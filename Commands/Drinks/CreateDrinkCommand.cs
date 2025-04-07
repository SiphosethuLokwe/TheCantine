using MediatR;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks
{

    public class CreateDrinkCommand : IRequest<Drink>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
