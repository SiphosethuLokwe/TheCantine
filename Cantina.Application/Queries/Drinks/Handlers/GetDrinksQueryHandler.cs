using Cantina.Domain;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;


namespace CantinaAPI.Queries.Drinks.Handlers
{
    public class GetDrinksQueryHandler : IRequestHandler<GetDrinksQuery, IEnumerable<Drink>>
    {
        private readonly IDrinkService _drinkService;

        public GetDrinksQueryHandler(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }
        public async Task<IEnumerable<Drink>> Handle(GetDrinksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _drinkService.GetAllAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
