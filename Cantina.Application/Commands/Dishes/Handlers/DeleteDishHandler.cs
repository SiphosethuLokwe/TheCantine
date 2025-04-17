using Cantina.Application.DTO;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cantina.Application.Commands.Dishes.Handlers
{
    public class DeleteDishHandler : IRequestHandler<DeleteDishCommand, CommandResponse<bool>>
    {
        private readonly IDishService _dishService;
        private readonly ILogger<DeleteDishHandler> _logger;    

        public DeleteDishHandler(IDishService dishService, ILogger<DeleteDishHandler> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {

            if (request.Id <= 0)
                throw new ArgumentNullException("Id is required");

            var dish = await _dishService.GetByIdAsync(request.Id, cancellationToken);
            if (dish == null)
            {
                return new CommandResponse<bool>
                {
                    Success = false,
                    Message = "Dish not found."
                };
            }

            await _dishService.DeleteAsync(request.Id, cancellationToken);
            return new CommandResponse<bool>
            {
                Success = true,
                Message = "Dish deleted successfully."
            };
        }     
        
    }
}