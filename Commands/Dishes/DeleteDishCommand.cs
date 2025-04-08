using MediatR;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes
{
    public class DeleteDishCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }

    }
}
