using Cantina.Application.Application.Commands;
using Cantina.Application.Commands.Dishes;
using Cantina.Application.DTO;
using Cantina.Application.Queries.Dishes;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CantinaAPI.Queries.Dishes;

namespace CantinaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogger<DishesController> _logger;

        public DishesController(IMediator mediator, ILogger<DishesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
           
                var query = new GetDishesQuery();
                var dishes = await _mediator.Send(query);
                return Ok(dishes);
               
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            
                var query = new GetDishByIdQuery { Id = id };
                var dish = await _mediator.Send(query);     
                return Ok(dish);
                
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<Dish>>> PostDish(CreateDishCommand command)
        {
               
                var dish = await _mediator.Send(command);
                return Ok(dish);  
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> PutDish(UpdateDishCommand command)
        {       
                   
               var isUpdated =  await _mediator.Send(command);
               return Ok(isUpdated);                  
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> DeleteDish(int id)
        {
                  
                var query = new DeleteDishCommand { Id = id };
                var dish = await _mediator.Send(query);
                return Ok(dish);
        }

        [HttpGet("search")]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<IEnumerable<Dish>>> SearchDishes([FromQuery] string searchTerm)
        {         
            var query = new SearchDishesQuery { SearchTerm = searchTerm };
            var dishes = await _mediator.Send(query);
            return Ok(dishes);
        }

    }
}