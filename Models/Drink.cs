using System.ComponentModel.DataAnnotations;

namespace TheCantine.Models
{
    public class Drink
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