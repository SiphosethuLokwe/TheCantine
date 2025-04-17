using Cantina.Application.DTO;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Cantina.Application.Commands.Dishes.Handlers
{
    public class UpdateDishHandler : IRequestHandler<UpdateDishCommand, CommandResponse<bool>>
    {
        private readonly IDishService _dishService;
        private readonly ILogger<UpdateDishHandler> _logger;

        public UpdateDishHandler(IDishService dishService, ILogger<UpdateDishHandler> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }
        public async Task<CommandResponse<bool>> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {

            if (request.Price <= 0) // we can even decimal try parse here but for now this is fine
                throw new ArgumentNullException("Price must be greater than 0");
            if (request.Id <= 0)
                throw new ArgumentNullException("Id not set");

            var dish = await _dishService.GetByIdAsync(request.Id, cancellationToken);
            if (dish == null)
            {
                return new CommandResponse<bool>
                {
                    Success = false,
                    Message = "Dish not found."
                };
            }

            dish.Name = request.Name ?? dish.Name;
            dish.Description = request.Description ?? dish.Description;
            dish.Price = request.Price > 0 ? request.Price : dish.Price;
            dish.Image = request.Image ?? dish.Image;

            await _dishService.UpdateAsync(dish, cancellationToken);
            return new CommandResponse<bool>
            {
                Success = true,
                Message = "Dish updated successfully."
            };
        }
           
    }
}