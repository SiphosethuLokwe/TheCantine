using Cantina.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Cantina.Application.Queries.Dishes.Handlers
{
    public class GetDishesQueryHandler : IRequestHandler<GetDishesQuery, IEnumerable<Dish>>
    {
        private readonly IDishService _dishService;
        private readonly ILogger<GetDishesQueryHandler> _logger;

        public GetDishesQueryHandler(IDishService dishService, ILogger<GetDishesQueryHandler> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }
        public async Task<IEnumerable<Dish>> Handle(GetDishesQuery request, CancellationToken cancellationToken)
        {                
           return await _dishService.GetAllAsync(request.Page, request.PageSize, cancellationToken);             
        }
    }
}
