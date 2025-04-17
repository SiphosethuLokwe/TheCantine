using Cantina.Application.DTO;
using Cantina.Domain.Entities;
using MediatR;


namespace Cantina.Application.Application.Commands
{
    public class CreateDishCommand : IRequest<CommandResponse<Dish>>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
