using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Filters;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    [RequireAdmin(OnlyForHandlers = new[] { "UpdateStatus", "Delete" })]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrdersModel(ApplicationDbContext context)
        {
            _context = context;
            Orders = new List<Order>();
        }

        public List<Order> Orders { get; set; } = new();
        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestEmail = HttpContext.Session.GetString("GuestEmail");

            // If no user ID or guest email, redirect to Order page instead of login
            if (userId == null && guestEmail == null)
            {
                return RedirectToPage("/Order");
            }

            // Check if user is admin
            IsAdmin = User.IsInRole("Admin");

            if (IsAdmin)
            {
                // Admin sees all orders including guest orders
                Orders = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Item)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            else
            {
                // Regular users and guests see only their orders
                Orders = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Item)
                    .Where(o => (userId != null && o.UserId == userId) || 
                              (guestEmail != null && o.GuestEmail == guestEmail))
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }

            return Page();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostDeleteAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
} 