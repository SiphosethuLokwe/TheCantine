using Cantina.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Queries.Reviews
{
    public class GetReviewsQuery : IRequest<IEnumerable<Review>>
    {
        public int? DishId { get; set; }
        public int? DrinkId { get; set; }

        public GetReviewsQuery(int? dishId, int? drinkId)
        {
            DishId = dishId;
            DrinkId = drinkId;
        }
    }
}
