using MediatR;
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
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            var query = new GetDishesQuery();
            var dishes = await _mediator.Send(query);
            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var query = new GetDishByIdQuery { Id = id };
            var dish = await _mediator.Send(query);
            if (dish == null)
            {
                return NotFound();
            }
            return Ok(dish);
        }

        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish(CreateDishCommand command)
        {
            var dish = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, UpdateDishCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var command = new DeleteDishCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}