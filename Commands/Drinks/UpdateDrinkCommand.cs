﻿using MediatR;

namespace TheCantine.Commands.Drinks
{
    public class UpdateDrinkCommand : IRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
