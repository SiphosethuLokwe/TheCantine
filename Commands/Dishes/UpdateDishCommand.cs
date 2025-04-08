﻿using MediatR;
using TheCantine.Models;

namespace TheCantine.Commands.Dishes
{
    public class UpdateDishCommand:IRequest<CommandResponse<bool>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
