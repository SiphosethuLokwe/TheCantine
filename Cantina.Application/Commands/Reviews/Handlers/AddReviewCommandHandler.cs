using Cantina.Application.Services;
using Cantina.Domain.Entities;
using MediatR;


namespace Cantina.Application.Commands.Reviews.Handlers
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Review>
    {
        private readonly IReviewService _reviewService;

        public AddReviewCommandHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public async Task<Review> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var review = new Review
                {
                    DishId = request.DishId,
                    DrinkId = request.DrinkId,
                    CustomerName = request.CustomerName,
                    Rating = request.Rating,
                    ReviewText = request.ReviewText
                };

                await _reviewService.CreateAsync(review, cancellationToken);
                return review;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
    }
}
