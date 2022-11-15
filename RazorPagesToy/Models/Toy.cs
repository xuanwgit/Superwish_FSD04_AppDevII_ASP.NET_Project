using System.ComponentModel.DataAnnotations;

namespace RazorPagesToy.Models
{
    public class Toy
    {
        public int ID { get; set; }

        public int categoryId { get; set;}

        public string name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int quantityRemaining { get; set; }

        public string ImageURL { get; set; } = string.Empty;
    }
}
