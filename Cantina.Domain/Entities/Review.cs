using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cantina.Domain.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int? DishId { get; set; }
        public int? DrinkId { get; set; }

        [Required, MaxLength(200)]
        public string CustomerName { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
