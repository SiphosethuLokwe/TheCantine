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
        public async Task<ActionResult<Review>> GetReview(int? id)
        {
            try
            { 
                // also update with a response and handle error in it handler/ also there is a cleaner way to write to build these problem child responses 
                if (id <= 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "BadRequest",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Invalid Id"
                    });
                }
                var query = new GetReviewByIdQuery { Id=id };
                var Review = await _mediator.Send(query);
                if (Review == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Not found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = "Review was not found"
                    });
                }
                return Ok(Review);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "FrontEnd")]
        public async Task<ActionResult<Review>> PostReview(AddReviewCommand command)
        {
            if (string.IsNullOrEmpty(command.CustomerName))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Customer name must be provided"
                });
            }
            try
            {

                var Review = await _mediator.Send(command);
                return Ok(Review);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
