using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Commands.Drinks;
using TheCantine.Models;
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
        public async Task<ActionResult<IEnumerable<Drink>>> GetDrinks()
        {
            var query = new GetDrinksQuery();
            var drinks = await _mediator.Send(query);
            return Ok(drinks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Drink>> GetDrink(int id)
        {
            var query = new GetDrinkByIdQuery { Id = id };
            var drink = await _mediator.Send(query);
            if (drink == null)
            {
                return NotFound();
            }
            return Ok(drink);
        }

        [HttpPost]
        public async Task<ActionResult<Drink>> PostDrink(CreateDrinkCommand command)
        {
            var drink = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDrink), new { id = drink.Id }, drink);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrink(int id, UpdateDrinkCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrink(int id)
        {
            var command = new DeleteDrinkCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
