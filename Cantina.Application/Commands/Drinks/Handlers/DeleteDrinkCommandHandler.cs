using Cantina.Application.Commands.Drinks;
using Cantina.Application.DTO;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CantinaAPI.Commands.Drinks.Handlers
{
    public class DeleteDrinkHandler : IRequestHandler<DeleteDrinkCommand, CommandResponse<bool>>
    {
        private readonly IDrinkService _drinkService;

        private readonly ILogger<DeleteDrinkHandler> _logger;

        public DeleteDrinkHandler(IDrinkService drinkhService, ILogger<DeleteDrinkHandler> logger)
        {
            _drinkService = drinkhService;
            _logger = logger;
        }
        public async Task<CommandResponse<bool>> Handle(DeleteDrinkCommand request, CancellationToken cancellationToken)
        {

                if (request.Id <= 0)
                throw new ArgumentNullException("id not set");
           
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
