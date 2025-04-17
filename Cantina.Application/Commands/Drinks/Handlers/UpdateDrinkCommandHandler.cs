using Cantina.Application.Commands.Drinks;
using Cantina.Application.DTO;
using Cantina.Domain;
using Cantina.Domain.Interfaces;
using MediatR;


namespace CantinaAPI.Commands.Drinks.Handlers
{
    public class UpdateDrinkHandler : IRequestHandler<UpdateDrinkCommand, CommandResponse<bool>>
    {
        private readonly IDrinkService _drinkService;

        public UpdateDrinkHandler(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public async Task<CommandResponse<bool>> Handle(UpdateDrinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Drink = await _drinkService.GetByIdAsync(request.Id, cancellationToken);
                if (Drink == null)
                {
                    return new CommandResponse<bool>
                    {
                        Success = false,
                        Message = "Drink not found."
                    };
                }

                Drink.Name = request.Name ?? Drink.Name;
                Drink.Description = request.Description ?? Drink.Description;
                Drink.Price = request.Price > 0 ? request.Price : Drink.Price;
                Drink.Image = request.Image ?? Drink.Image;

                await _drinkService.UpdateAsync(Drink, cancellationToken);
                return new CommandResponse<bool>
                {
                    Success = true,
                    Message = "Drink updated successfully."
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }
        }
    }
}
