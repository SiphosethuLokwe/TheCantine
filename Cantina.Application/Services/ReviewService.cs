using Cantina.Application.Interfaces;
using Cantina.Domain.Entities;

namespace Cantina.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;

        public ReviewService(IGenericRepository<Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _reviewRepository.GetAllAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving reviews.", ex);
            }
        }

        public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _reviewRepository.GetByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve review with ID {id}.", ex);
            }
        }

        public async Task CreateAsync(Review review, CancellationToken cancellationToken)
        {
            try
            {
                await _reviewRepository.AddAsync(review, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create review.", ex);
            }
        }

        public async Task<IEnumerable<Review>> GetByTypeIdAsync(int? dishId, int? drinkId, CancellationToken cancellationToken)
        {
      
           throw new ApplicationException("Failed to retrieve reviews for the specified type.");
        }
    }
}
