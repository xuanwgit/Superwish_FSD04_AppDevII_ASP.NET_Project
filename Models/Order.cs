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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public IdentityUser User_Id { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(5000)]
        public string PaymentCode { get; set; }
    }
}