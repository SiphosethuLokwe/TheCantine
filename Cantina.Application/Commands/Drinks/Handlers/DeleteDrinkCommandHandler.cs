using Cantina.Application.Commands.Drinks;
using Cantina.Application.DTO;
using MediatR;


namespace CantinaAPI.Commands.Drinks.Handlers
{
    public class DeleteDrinkHandler : IRequestHandler<DeleteDrinkCommand, CommandResponse<bool>>
    {
        private readonly IDrinkService _drinkService;

        public DeleteDrinkHandler(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteDrinkCommand request, CancellationToken cancellationToken)
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

                await _drinkService.DeleteAsync(request.Id, cancellationToken);
                return new CommandResponse<bool>
                {
                    Success = true,
                    Message = "Drink deleted successfully."
                };           
          
        }
    }
}
