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
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string GuestEmail { get; set; } = string.Empty;

        [Display(Name = "Card Number")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits")]
        public string CardNumber { get; set; } = string.Empty;

        [Display(Name = "Card Holder Name")]
        public string CardHolderName { get; set; } = string.Empty;

        [Display(Name = "Expiry Month")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Range(2024, 2100, ErrorMessage = "Year must be between 2024 and 2100")]
        public int ExpiryYear { get; set; }

        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; } = string.Empty;

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State/Province is required")]
        [Display(Name = "State/Province")]
        public string StateProvince { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; } = string.Empty;

        // Payment information
        public string? PaymentReferenceId { get; set; } // Store payment processor reference
        public string? PaymentStatus { get; set; } // Store payment status

        // Optional notes
        public string? Notes { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public string GetFormattedAddress()
        {
            var addressParts = new List<string> { AddressLine1 };
            
            if (!string.IsNullOrWhiteSpace(AddressLine2))
                addressParts.Add(AddressLine2);
            
            addressParts.Add($"{City}, {StateProvince} {PostalCode}");
            addressParts.Add(Country);

            return string.Join("\n", addressParts);
        }

        public string GetMaskedCardNumber()
        {
            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length < 4)
                return "****";
            return "**** **** **** " + CardNumber.Substring(CardNumber.Length - 4);
        }
    }
}