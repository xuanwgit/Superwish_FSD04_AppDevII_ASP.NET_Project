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
    public class Cart_Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public IdentityUser User { get; set; } = null!;

        [Required]
        public int ItemId { get; set; }

        [Required]
        public Item Item { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }
    }
}