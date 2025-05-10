using Cantina.Domain.Entities;

namespace Cantina.Application.Services
{
    public interface IReviewService
    {
        Task CreateAsync(Review review, CancellationToken cancellationToken);
        Task<IEnumerable<Review>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Review>> GetByTypeIdAsync(int? dishId, int? drinkId, CancellationToken cancellationToken);
    }
}