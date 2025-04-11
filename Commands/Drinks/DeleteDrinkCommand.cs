using MediatR;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks
{
    public class DeleteDrinkCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }
    }
}
