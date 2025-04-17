using Cantina.Application.Services;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;

namespace Cantina.Application.Queries.Reviews.Handlers
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<Review>>
    {
        private readonly IReviewService _reviewRepository;

        public GetReviewsQueryHandler(IReviewService ReviewRepository)
        {
            _reviewRepository = ReviewRepository;
        }

        public async Task<IEnumerable<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _reviewRepository.GetByTypeIdAsync(request.DishId, request.DrinkId, cancellationToken);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
