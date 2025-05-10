using Cantina.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Queries.Dishes.Handlers
{
    public class SearchDishesQueryHandler : IRequestHandler<SearchDishesQuery, IEnumerable<Dish>>
    {
        private readonly IDishService _dishService;

        public SearchDishesQueryHandler(IDishService dishService)
        {
            _dishService = dishService;
        }

        public async Task<IEnumerable<Dish>> Handle(SearchDishesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.SearchTerm))
            {
                throw new ArgumentNullException("Search term cannot be empty.");
            }
            return await _dishService.SearchAsync(request.SearchTerm, cancellationToken);
        }
    }

}
