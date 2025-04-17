using Cantina.Application.Application.Commands;
using Cantina.Application.Commands.Drinks;
using Cantina.Application.DTO;
using Cantina.Domain;
using Cantina.Domain.Entities;
using MediatR;


namespace Cantina.Application.Commands.drinkes.Handlers
{
    public class CreatedrinkHandler : IRequestHandler<CreateDrinkCommand, CommandResponse<Drink>>
    {
        private readonly IDrinkService _drinkService;

        public CreatedrinkHandler(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }
        public async Task<CommandResponse<Drink>> Handle(CreateDrinkCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.Name)) // also perhaps a generic validation helper to validate common fields(later improvements)
                throw new ArgumentNullException("name must be provided");

            if (string.IsNullOrEmpty(request.Description)) 
                throw new ArgumentNullException("name must be provided");

            if (request.Price <= 0) 
                throw new ArgumentNullException("Please provide price");

            var existingDrink = await _drinkService.GetByNameAsync(request.Name, cancellationToken);
                if (existingDrink != null)
                {
                    return new CommandResponse<Drink>
                    {
                        Success = false,
                        Message = "Drink already exists."
                    };
                }
                var drink = new Drink
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = request.Image
                };

                await _drinkService.CreateAsync(drink,cancellationToken);
                return new CommandResponse<Drink>
                {
                    Success = true,
                    Message = "drink created successfully.",
                    Data = drink
                };
        }
           
        
    }
}