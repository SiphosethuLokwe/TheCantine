using Cantina.Application.DTO;
using MediatR;

namespace Cantina.Application.Commands.Dishes
{
    public class DeleteDishCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }

    }
}
