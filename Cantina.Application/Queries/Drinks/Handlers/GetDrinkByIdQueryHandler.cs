using Cantina.Domain;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace CantinaAPI.Queries.Drinks.Handlers
{
    public class GetDrinkByIdQueryHandler : IRequestHandler<GetDrinkByIdQuery, Drink>
    {
        private readonly IDrinkService _drinkService;
        private readonly ILogger<GetDrinkByIdQueryHandler> _logger;


        public GetDrinkByIdQueryHandler(IDrinkService drinkService, ILogger<GetDrinkByIdQueryHandler> logger )
        {
            _drinkService = drinkService;
            _logger = logger;
        }
        public async Task<Drink> Handle(GetDrinkByIdQuery request, CancellationToken cancellationToken)
        {

            if (request.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Id), "No Id was provided");            
              return await _drinkService.GetByIdAsync(request.Id, cancellationToken);           
           
        }
    }
}
