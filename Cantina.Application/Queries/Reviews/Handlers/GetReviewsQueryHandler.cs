using Cantina.Application.Services;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cantina.Application.Queries.Reviews.Handlers
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<Review>>
    {
        private readonly IReviewService _reviewRepository;
        private readonly ILogger<GetReviewsQueryHandler> _logger;

        public GetReviewsQueryHandler(IReviewService ReviewRepository, ILogger<GetReviewsQueryHandler> logger)
        {
            _reviewRepository = ReviewRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
     
           return await _reviewRepository.GetByTypeIdAsync(request.DishId, request.DrinkId, cancellationToken);

        }
    }
}
