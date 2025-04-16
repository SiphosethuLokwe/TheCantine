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
          
                var query = new GetDrinkByIdQuery { Id = id };
                var drink = await _mediator.Send(query);         
                return Ok(drink);      
          
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<Drink>>> PostDrink(CreateDrinkCommand command)
        {
 
             var drink = await _mediator.Send(command);
             return Ok(drink);                
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> PutDrink( UpdateDrinkCommand command)
        {         
           
             var isUpdated = await _mediator.Send(command);
             return Ok(isUpdated);
           
          
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> DeleteDrink(int id)
        {        
                var query = new DeleteDrinkCommand { Id = id };
                var result = await _mediator.Send(query);
                return Ok(result);
                    
        }
    }
}
