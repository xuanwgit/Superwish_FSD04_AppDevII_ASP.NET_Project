using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? ImageUrl { get; set; }

        // Navigation property
        public ICollection<Item> Items { get; set; } = new List<Item>();

        // Default descriptions for categories
        public static readonly Dictionary<string, string> DefaultDescriptions = new()
        {
            {
                "Plushes",
                "Discover our collection of soft, cuddly plush toys perfect for all ages! " +
                "From adorable animals to beloved characters, our plushies are made with premium materials " +
                "for maximum comfort and durability. Each plush toy is carefully crafted with attention to detail, " +
                "making them perfect companions for playtime, bedtime, or as decorative pieces. " +
                "Our plush collection features a variety of sizes, from pocket-sized friends to large huggable companions, " +
                "ensuring there's a perfect plush for everyone."
            },
            {
                "PRINTS",
                "Explore our stunning collection of high-quality art prints! " +
                "Each print is carefully reproduced from original artwork, ensuring vibrant colors and crisp details. " +
                "Our prints are available in various sizes and formats, perfect for adding a touch of personality to any space. " +
                "Printed on premium paper with archival-quality inks, these pieces are designed to last and maintain their beauty over time. " +
                "Whether you're looking for a statement piece for your living room or a subtle accent for your office, " +
                "our print collection offers something for every style and preference."
            },
            {
                "PUSH TOYS",
                "Discover our engaging collection of push toys designed to encourage movement and development! " +
                "Our push toys are crafted with safety and durability in mind, featuring sturdy construction and smooth-rolling wheels. " +
                "Perfect for toddlers learning to walk, these toys provide stability and confidence while promoting physical activity. " +
                "Each push toy is designed with bright colors, fun sounds, and interactive elements to stimulate sensory development. " +
                "From classic walkers to modern push-along vehicles, our collection offers a variety of options to support your child's growth and exploration."
            }
        };
    }
}