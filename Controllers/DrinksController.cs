using Cantina.Application.Commands.Drinks;
using Cantina.Application.DTO;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Queries.Drinks;


namespace TheCantine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DrinksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<IEnumerable<Drink>>> GetDrinks()
        {
            var query = new GetDrinksQuery();
            var drinks = await _mediator.Send(query);
            return Ok(drinks);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "FrontEnd")]
        public async Task<ActionResult<Drink>> GetDrink(int id)
        {
            try
            { // also update with a response and handle error in it handler/ also there is a cleaner way to write to build these problem child responses 
                if (id <= 0)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "BadRequest",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Invalid Id"
                    });
                }
                var query = new GetDrinkByIdQuery { Id = id };
                var drink = await _mediator.Send(query);
                if (drink == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Not found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = "Drink was not found"
                    });
                }
                return Ok(drink);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<Drink>>> PostDrink(CreateDrinkCommand command)
        {
            if (string.IsNullOrEmpty(command.Name))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Name of drink must be provided"
                });
            }
            if (command.Price <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Price must be greater than 0"
                });
            }
            try
            {

                var drink = await _mediator.Send(command);
                return Ok(drink);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> PutDrink( UpdateDrinkCommand command)
        {
            if (command.Id <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid Id"
                });
            }
            if (command.Price <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Price must be greater than 0"
                });
            }
            try
            {
                var isUpdated = await _mediator.Send(command);
                return Ok(isUpdated);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> DeleteDrink(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid Id"
                });
            }
            try
            {
                var query = new DeleteDrinkCommand { Id = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
