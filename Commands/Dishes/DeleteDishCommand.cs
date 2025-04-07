using MediatR;

namespace TheCantine.Commands.Dishes
{
    public class DeleteDishCommand : IRequest
    {
        public int Id { get; set; }

    }
}
