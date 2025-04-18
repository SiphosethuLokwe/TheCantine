﻿using Cantina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CantinaAPI.Infrastructure.Data
{
    public class CantinaContext : DbContext
    {
        public CantinaContext(DbContextOptions<CantinaContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
