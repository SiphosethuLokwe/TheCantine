using Cantina.Application.Services;
using Cantina.Domain;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Application.Queries.Reviews.Handlers
{
    public class GetReviewsByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, Review>
    {
        private readonly IReviewService _reviewRepository;

        public GetReviewsByIdQueryHandler(IReviewService ReviewRepository)
        {
            _reviewRepository = ReviewRepository;
        }
        public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _reviewRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
