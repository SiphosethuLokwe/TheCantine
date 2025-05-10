using Cantina.Domain;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CantinaAPI.Queries.Drinks.Handlers
{
    public class GetDrinksQueryHandler : IRequestHandler<GetDrinksQuery, IEnumerable<Drink>>
    {
        private readonly IDrinkService _drinkService;
        private readonly ILogger<GetDrinksQueryHandler> _logger;

        public GetDrinksQueryHandler(IDrinkService drinkService, ILogger<GetDrinksQueryHandler> logger)
        {
            _drinkService = drinkService;
            _logger = logger;
        }
        public async Task<IEnumerable<Drink>> Handle(GetDrinksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting Drinks......");
            return await _drinkService.GetAllAsync(request.Page, request.PageSize,cancellationToken);
       
        }
    }
}
