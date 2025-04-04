using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestEmail = HttpContext.Session.GetString("GuestEmail");

            Order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == orderId && 
                    ((userId != null && o.UserId == userId) || 
                     (o.GuestEmail != null && o.GuestEmail == guestEmail)));

            if (Order == null)
            {
                return RedirectToPage("/Cart");
            }

            // Check if this is a successful order submission
            if (TempData["OrderSuccess"] == null)
            {
                return RedirectToPage("/Cart");
            }

            // Clear the OrderSuccess flag after using it
            TempData.Remove("OrderSuccess");

            return Page();
        }
    }
} 