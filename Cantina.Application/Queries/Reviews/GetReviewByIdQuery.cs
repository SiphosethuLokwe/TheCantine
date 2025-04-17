using Cantina.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Queries.Reviews
{
    public  class GetReviewByIdQuery: IRequest<Review>
    {
        public int Id { get; set; }

    }
}
