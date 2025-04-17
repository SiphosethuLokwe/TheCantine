using Cantina.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Commands.Reviews
{
    public class AddReviewCommand : IRequest<Review>
    {
        public int? DishId { get; set; }
        public int? DrinkId { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
