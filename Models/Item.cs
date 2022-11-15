using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Category CategoryFk { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int QuantityRemaining { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ImageUrl { get; set; }
    }
        
}