using MediatR;
using Microsoft.EntityFrameworkCore;
using TheCantine.Data;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes.Handlers
{
    public class DeleteDishHandler : IRequestHandler<DeleteDishCommand, CommandResponse<bool>>
    {
        private readonly CantinaContext _context;

        public DeleteDishHandler(CantinaContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dish = await _context.Dishes.FindAsync(request.Id);
                if (dish == null)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Drink not found."
                    };

                }

    
                 _context.Entry(dish).State = EntityState.Modified;
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
