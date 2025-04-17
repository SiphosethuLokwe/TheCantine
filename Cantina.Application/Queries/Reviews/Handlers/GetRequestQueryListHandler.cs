using Cantina.Domain.Entities;
using MediatR;
using Cantina.Domain.Interfaces;
using Cantina.Application.Services;
using Microsoft.Extensions.Logging;

namespace Cantina.Application.Queries.Reviews.Handlers
{
    public class GetReviewListQueryHandler : IRequestHandler<GetReviewList, IEnumerable<Review>>
    {
        private readonly IReviewService _reviewRepository;
        private readonly ILogger<GetReviewListQueryHandler> _logger;

        public GetReviewListQueryHandler(IReviewService ReviewRepository, ILogger<GetReviewListQueryHandler> logger)
        {
            _reviewRepository = ReviewRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Review>> Handle(GetReviewList request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting reviews....");
           return await _reviewRepository.GetAllAsync(cancellationToken);
        
        }
    }
}
