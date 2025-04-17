using Cantina.Application.Services;
using Cantina.Domain;
using Cantina.Domain.Entities;
using Cantina.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GetReviewsByIdQueryHandler> _logger;

        public GetReviewsByIdQueryHandler(IReviewService ReviewRepository, ILogger<GetReviewsByIdQueryHandler> logger)
        {
            _reviewRepository = ReviewRepository;
            _logger = logger;

        }
        public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting review by id....");
            return await _reviewRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

         
        }
    }
}
