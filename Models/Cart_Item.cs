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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public IdentityUser userId { get; set; }
        [Required]
        public Item itemId { get; set; }
        [Required]
        public int quantity { get; set; }
    }
}