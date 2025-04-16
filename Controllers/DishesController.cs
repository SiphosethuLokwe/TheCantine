using Cantina.Application.Application.Commands;
using Cantina.Application.Commands.Dishes;
using Cantina.Application.DTO;
using Cantina.Application.Queries.Dishes;
using Cantina.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Queries.Dishes;

namespace TheCantine.Controllers
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
            try
            {
                var query = new GetDishesQuery();
                var dishes = await _mediator.Send(query);
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "FrontEnd,Admin")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            try
            { 
                var query = new GetDishByIdQuery { Id = id };
                var dish = await _mediator.Send(query);     
                return Ok(dish);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "Internal server error");
            }
                
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<Dish>>> PostDish(CreateDishCommand command)
        {
      
            try
            {    
                var dish = await _mediator.Send(command);
                return Ok(dish);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "Internal server error");
            }
                
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> PutDish(UpdateDishCommand command)
        {
          
            try
            {            
               var isUpdated =  await _mediator.Send(command);
               return Ok(isUpdated);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "Internal server error");
            }
              
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CommandResponse<bool>>> DeleteDish(int id)
        {
          
            try
            {
                var query = new DeleteDishCommand { Id = id };
                var dish = await _mediator.Send(query);
                return Ok(dish);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, "Internal server error");
            }
       
        }
    }
}