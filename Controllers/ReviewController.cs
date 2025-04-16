using Cantina.Application.Commands.Reviews;
using Cantina.Application.DTO;
using Cantina.Application.Queries.Reviews;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Queries.Drinks;

namespace TheCantine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var query = new GetReviewList();
            var Reviews = await _mediator.Send(query);
            return Ok(Reviews);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
        
                var query = new GetReviewByIdQuery { Id=id };
                var Review = await _mediator.Send(query);        
                return Ok(Review);         
          
        }

        [HttpPost]
        [Authorize(Roles = "FrontEnd")]
        public async Task<ActionResult<Review>> PostReview(AddReviewCommand command)
        {     

                var Review = await _mediator.Send(command);
                return Ok(Review);

        }
    }
}
