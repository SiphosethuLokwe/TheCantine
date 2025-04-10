using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Commands.Dishes;
using TheCantine.Models;
using TheCantine.Queries.Dishes;

namespace TheCantine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DishesController(IMediator mediator)
        {
            //inject Ilogger
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            try
            {
                var query = new GetDishesQuery();
                var dishes = await _mediator.Send(query);
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<Dish>> GetDish(int id)
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
                var query = new GetDishByIdQuery { Id = id };
                var dish = await _mediator.Send(query);
                if (dish == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Not found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = "Dish was not found"
                    });
                }
                return Ok(dish);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
                
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<Dish>>> PostDish(CreateDishCommand command)
        {
            //possibly move my validation away from here my controller is getting a bit too much 
            if (string.IsNullOrEmpty(command.Name))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Name of dish must be provided"
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
           
                var dish = await _mediator.Send(command);
                return Ok(dish);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
                
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> PutDish(int id, UpdateDishCommand command)
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
               var isUpdated =  await _mediator.Send(command);
               return Ok(isUpdated);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
              
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> DeleteDish(int id)
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
                var query = new GetDishByIdQuery { Id = id };
                var dish = await _mediator.Send(query);
                return Ok(dish);
            }
            catch (Exception ex)
            {
                // Log later
                return StatusCode(500, "Internal server error");
            }
       
        }
    }
}