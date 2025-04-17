using Cantina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Interfaces
{

    public interface IReviewRepository
    {

        Task<IEnumerable<Review>> GetReviewsAsync(CancellationToken cancellation);
        Task<Review> GetReviewByIdAsync(int? id, CancellationToken cancellation);
        Task<IEnumerable<Review>> GetReviewsByTypeId(int? dishId, int? drinkId, CancellationToken cancellation);
        Task CreateReviewAsync(Review review);

    }
}
