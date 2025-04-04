using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? GuestCartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Item? Item { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
} 