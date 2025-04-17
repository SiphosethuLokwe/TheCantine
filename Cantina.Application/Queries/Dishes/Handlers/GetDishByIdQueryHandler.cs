using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using CantinaAPI.Queries.Dishes;

namespace Cantina.Application.Queries.Dishes.Handlers
{
    public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, Dish>
    {
        private readonly IDishService _dishService;
        private readonly ILogger<GetDishByIdQueryHandler> _logger;

        public GetDishByIdQueryHandler(IDishService dishService, ILogger<GetDishByIdQueryHandler> logger )
        {
            _dishService = dishService;
            _logger = logger;
        }
        public async Task<Dish> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
          
                if (request.Id <= 0)              
                   throw new ArgumentNullException("Id must be greater than 0");
                
                var result = await _dishService.GetByIdAsync(request.Id, cancellationToken);
                if (result == null) 
                    throw new ArgumentNullException("Dish was not found"); 
               
                return result;

        }
    }
}
