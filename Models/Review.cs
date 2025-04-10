namespace TheCantine.Models
{
    public class Review
    {
        public int Id { get; set; } 
        public int itemId { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
