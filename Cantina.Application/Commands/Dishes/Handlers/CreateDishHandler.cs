using Cantina.Application.Application.Commands;
using Cantina.Application.DTO;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Cantina.Application.Commands.Dishes.Handlers
{
    public class CreateDishHandler : IRequestHandler<CreateDishCommand, CommandResponse<Dish>>
    {
        private readonly IDishService _dishService;
        private readonly ILogger<CreateDishHandler> _logger;

        public CreateDishHandler(IDishService dishService, ILogger<CreateDishHandler> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }

        public async Task<CommandResponse<Dish>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {

               if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentNullException("Dish name is required");
                if (string.IsNullOrEmpty(request.Description))
                    throw new ArgumentNullException("Description is required");
                
                if (request.Price <= 0)
                    throw new ArgumentOutOfRangeException("Price must be greater than 0");


                var existingDrink = await _dishService.GetByNameAsync(request.Name, cancellationToken);
                if (existingDrink != null)
                {
                    return new CommandResponse<Dish>
                    {
                        Success = false,
                        Message = "Drink already exists."
                    };
                }
                var dish = new Dish
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = request.Image
                };

                await _dishService.CreateAsync(dish, cancellationToken);
                return new CommandResponse<Dish>
                {
                    Success = true,
                    Message = "Dish created successfully.",
                    Data = dish
                }; 
          
        }
    }
}