using MediatR;

namespace TheCantine.Commands.Drinks
{
    public class DeleteDrinkCommand : IRequest
    {
        public int Id { get; set; }
    }
}
