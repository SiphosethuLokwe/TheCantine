using MediatR;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Drinks.Handlers
{
    public class DeleteDrinkCommandHandler : IRequestHandler<DeleteDrinkCommand, CommandResponse<bool>>
    {
        private readonly CantinaContext _context;

        public DeleteDrinkCommandHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteDrinkCommand request, CancellationToken cancellationToken)
        {
           try
            {
                var drink = await _context.Drinks.FindAsync(request.Id);
                if (drink == null)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Drink not found."
                    };

                }
                _context.Drinks.Remove(drink);
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "No Rows were affected"
                    };
                }
                return new CommandResponse<bool>
                {
                    Success = true,
                    Message = "Drink deleted successfully"
                };
            }
            catch (Exception ex)
            {
                //logging will add later
                throw;
            }
        }
    }
}
