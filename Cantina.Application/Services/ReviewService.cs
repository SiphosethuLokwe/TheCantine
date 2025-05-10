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

        public async Task<IEnumerable<Review>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            int skip = (page - 1) * pageSize;
            return await _reviewRepository.GetAllAsync(skip, pageSize, cancellationToken);
                        
        }

        public async Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            
            return await _reviewRepository.GetByIdAsync(id, cancellationToken);
                       
        }

        public async Task CreateAsync(Review review, CancellationToken cancellationToken)
        {           
            await _reviewRepository.AddAsync(review, cancellationToken);                      
        }

        public async Task<IEnumerable<Review>> GetByTypeIdAsync(int? dishId, int? drinkId, CancellationToken cancellationToken)
        {
      
           throw new ApplicationException("Failed to retrieve reviews for the specified type.");
        }
    }
}
