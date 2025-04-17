using Cantina.Domain.Entities;
using MediatR;
using Cantina.Domain.Interfaces;
using Cantina.Application.Services;

namespace Cantina.Application.Queries.Reviews.Handlers
{
    public class GetReviewListQueryHandler : IRequestHandler<GetReviewList, IEnumerable<Review>>
    {
        private readonly IReviewService _reviewRepository;

        public GetReviewListQueryHandler(IReviewService ReviewRepository)
        {
            _reviewRepository = ReviewRepository;
        }
        public async Task<IEnumerable<Review>> Handle(GetReviewList request, CancellationToken cancellationToken)
        {
            try
            {
                return await _reviewRepository.GetAllAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
