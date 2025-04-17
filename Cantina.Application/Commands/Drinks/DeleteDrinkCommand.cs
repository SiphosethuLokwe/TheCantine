using Cantina.Application.DTO;
using MediatR;

namespace Cantina.Application.Commands.Drinks
{
    
    public class DeleteDrinkCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }
    }
}
