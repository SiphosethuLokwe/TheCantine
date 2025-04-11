 The Cantine Project Development Documentation

## Overview

The Cantine is a web application designed to manage dishes and drinks for a cantina. The application is built using ASP.NET Core and follows the principles of Clean Architecture. It uses CQRS (Command Query Responsibility Segregation) and MediatR for handling commands and queries. The application also integrates with a separate security microservice for authentication and authorization. The frontend of the application is built using Vue 3.

## Architecture

The Cantine project is structured using Clean Architecture, which ensures a clear separation of concerns and promotes maintainability, testability, and scalability. The project is divided into four main layers:

1. **Presentation Layer (TheCantine.Api)**: Handles HTTP requests and responses.
2. **Application Layer (TheCantine.Application)**: Contains the application logic, including commands, queries, and handlers.
3. **Domain Layer (TheCantine.Domain)**: Contains the core business logic and domain entities.
4. **Infrastructure Layer (TheCantine.Infrastructure)**: Contains data access and repository implementations.

### Project Structure

src/
├── TheCantine.Api/                # Presentation Layer
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── TheCantine.Application/        # Application Layer
│   ├── Commands/
│   ├── Queries/
│   ├── Handlers/
│   ├── DTO/
│   └── Services/
├── TheCantine.Domain/             # Domain Layer
│   ├── Entities/
│   ├── ValueObjects/
│   └── Interfaces/
├── TheCantine.Infrastructure/     # Infrastructure Layer
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
│   └── Configurations/
└── TheCantine.Tests/              # Test Project
    ├── UnitTests/
    └── IntegrationTests/

Clean Architecture
Clean Architecture ensures that the core business logic and domain entities are independent of external concerns such as data access and presentation. This is achieved by organizing the code into distinct layers with clear responsibilities.
•	Domain Layer: Contains the core business logic and domain entities.
•	Application Layer: Contains the application logic, including use cases, commands, queries, and handlers.
•	Infrastructure Layer: Contains the implementation details, such as data access, external services, and repositories.
•	Presentation Layer: Contains the user interface, such as API controllers.

CQRS and MediatR
CQRS (Command Query Responsibility Segregation) is a pattern that separates read and write operations into different models. Commands are used to change the state of the system, while queries are used to retrieve data.
MediatR is a library that facilitates the implementation of CQRS by providing a mediator pattern for handling commands and queries. It decouples the sender and receiver of a request, allowing for more maintainable and testable code.
Security Microservice
TheCantine application integrates with a separate security microservice for authentication and authorization. The security microservice is responsible for generating and validating JWT tokens, which are used to secure the API endpoints.

Communication Diagram
Below is a diagram illustrating how the components of TheCantine application communicate with each other and the security microservice:

+-------------------+       +-------------------+       +-------------------+
|                   |       |                   |       |                   |
|  Presentation     |       |  Application      |       |  Domain           |
|  Layer            |       |  Layer            |       |  Layer            |
|  (TheCantine.Api) |       |  (TheCantine.App) |       |  (TheCantine.Dom) |
|                   |       |                   |       |                   |
+--------+----------+       +--------+----------+       +--------+----------+
         |                           |                           |
         |                           |                           |
         v                           v                           v
+--------+----------+       +--------+----------+       +--------+----------+
|                   |       |                   |       |                   |
|  Infrastructure   |       |  MediatR          |       |  Entities         |
|  Layer            |       |  (Commands/       |       |  (Dish, Drink)    |
|  (TheCantine.Inf) |       |  Queries/Handlers)|       |                   |
|                   |       |                   |       |                   |
+--------+----------+       +--------+----------+       +--------+----------+
         |                           |
         |                           |
         v                           v
+--------+----------+       +--------+----------+
|                   |       |                   |
|  Security         |       |  Database         |
|  Microservice     |       |  (SQLite)         |
|  (Auth Server)    |       |                   |
|                   |       |                   |
+-------------------+       +-------------------+


Presentation Layer (TheCantine.Api)
The presentation layer handles HTTP requests and responses. It contains API controllers that interact with the application layer using MediatR.
Example: DishesController.cs


using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCantine.Application.Commands.Dishes;
using TheCantine.Application.DTO;
using TheCantine.Application.Queries.Dishes;
using TheCantine.Domain.Entities;

namespace TheCantine.Api.Controllers
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
        public async Task<ActionResult<CommandResponse<bool>>> PutDish(UpdateDishCommand command)
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
                var command = new DeleteDishCommand { Id = id };
                var result = await _mediator.Send(command);
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

Application Layer (TheCantine.Application)
The application layer contains the application logic, including commands, queries, and handlers. It uses MediatR to handle commands and queries.
Example: CreateDishCommand.cs
using MediatR;

using TheCantine.Domain.Entities;

namespace TheCantine.Application.Commands.Dishes
{
    public class CreateDishCommand : IRequest<CommandResponse<Dish>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
Example: CreateDishCommandHandler.cs
using MediatR;
using TheCantine.Domain.Entities;
using TheCantine.Domain.Interfaces;

namespace TheCantine.Application.Handlers
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, CommandResponse<Dish>>
    {
        private readonly IDishRepository _dishRepository;

        public CreateDishCommandHandler(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<CommandResponse<Dish>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            var dish = new Dish
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Image = request.Image
            };

            await _dishRepository.AddAsync(dish);

            return new CommandResponse<Dish>(dish, true, "Dish created successfully");
        }
    }
}
Domain Layer (Cantine.Domain)
The domain layer contains the core business logic and domain entities.
Example: Dish.cs
using System.ComponentModel.DataAnnotations;

namespace TheCantine.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        [Url]
        public string Image { get; set; }
    }
}
Infrastructure Layer (TheCantine.Infrastructure)
The infrastructure layer contains data access and repository implementations.
Example: DishRepository.cs
using Microsoft.EntityFrameworkCore;
using TheCantine.Domain.Entities;
using TheCantine.Domain.Interfaces;

namespace TheCantine.Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly CantinaContext _context;

        public DishRepository(CantinaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task AddAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }
    }
}

Security Microservice
The security microservice is responsible for generating and validating JWT tokens. TheCantine application uses these tokens to secure its API endpoints.
Example: BearerTokenMiddleware.cs

public class BearerTokenMiddleware
{
    private readonly RequestDelegate _next;

    public BearerTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.Headers["Authorization"] = "Bearer " + authHeader;
            }
        }

        await _next(context);
    }
}
Frontend (Vue 3)
The frontend of TheCantine application is built using Vue 3. It communicates with the backend API to manage dishes and drinks. The frontend uses Axios to make HTTP requests and handles authentication using JWT tokens.
Example: Axios Request in Vue 3
import axios from 'axios';

const token = 'your-jwt-token';

axios.get('https://localhost:7224/api/Dishes', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'accept': 'application/json'
  }
})
.then(response => {
  console.log(response.data);
})
.catch(error => {
  console.error('Error:', error);
});
