using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class OrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderModel> _logger;

        public OrderModel(ApplicationDbContext context, ILogger<OrderModel> logger)
        {
            _context = context;
            _logger = logger;
            Order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                PaymentStatus = "Pending"
            };
        }

        public List<CartItem> Items { get; set; } = new();
        
        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            // If no user ID or guest cart ID, redirect to Cart page
            if (userId == null && guestCartId == null)
            {
                return RedirectToPage("/Cart");
            }

            Items = await _context.CartItems
                .Include(c => c.Item)
                .Where(c => (userId != null && c.UserId == userId) || 
                          (guestCartId != null && c.GuestCartId == guestCartId))
                .ToListAsync();

            if (!Items.Any())
            {
                return RedirectToPage("/Cart");
            }

            Order = new Order
            {
                UserId = userId,
                GuestEmail = HttpContext.Session.GetString("GuestEmail") ?? string.Empty,
                OrderDate = DateTime.UtcNow,
                TotalAmount = Items.Sum(x => x.Quantity * (x.Item?.Price ?? 0))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            // If no user ID or guest cart ID, redirect to Cart page
            if (userId == null && guestCartId == null)
            {
                return RedirectToPage("/Cart");
            }

            Items = await _context.CartItems
                .Include(c => c.Item)
                .Where(c => (userId != null && c.UserId == userId) || 
                          (guestCartId != null && c.GuestCartId == guestCartId))
                .ToListAsync();

            if (!Items.Any())
            {
                return RedirectToPage("/Cart");
            }

            // Clear ModelState for fields we're setting programmatically
            ModelState.Remove("Order.UserId");
            ModelState.Remove("Order.ExpiryMonth");
            ModelState.Remove("Order.ExpiryYear");
            ModelState.Remove("Order.CardNumber");
            ModelState.Remove("Order.CardHolderName");
            ModelState.Remove("Order.CVV");

            // Set required values
            Order.UserId = userId;
            
            // For guest users, store their email in the session
            if (userId == null && !string.IsNullOrEmpty(Order.GuestEmail))
            {
                HttpContext.Session.SetString("GuestEmail", Order.GuestEmail);
            }
            else
            {
                Order.GuestEmail = HttpContext.Session.GetString("GuestEmail") ?? string.Empty;
            }

            Order.CardNumber = "0000000000000000";
            Order.CardHolderName = "Test Payment";
            Order.ExpiryMonth = 12;
            Order.ExpiryYear = 2024;
            Order.CVV = "000";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check stock availability for all items
            foreach (var cartItem in Items)
            {
                if (cartItem.Item == null || cartItem.Item.QuantityRemaining < cartItem.Quantity)
                {
                    ModelState.AddModelError("", $"Sorry, there are only {cartItem.Item?.QuantityRemaining} units available for {cartItem.Item?.Name}.");
                    return Page();
                }
            }

            // Update order details
            Order.OrderDate = DateTime.UtcNow;
            Order.TotalAmount = Items.Sum(x => x.Quantity * (x.Item?.Price ?? 0));
            Order.Total = Order.TotalAmount;
            Order.Status = "Pending";
            Order.PaymentStatus = "Pending";

            try
            {
                // Simulate payment processing
                Order.PaymentReferenceId = "TEST_" + Guid.NewGuid().ToString();
                Order.PaymentStatus = "Completed";
                Order.Status = "Processing";

                _context.Orders.Add(Order);
                await _context.SaveChangesAsync();

                // Create order items, update inventory, and clear cart
                foreach (var cartItem in Items)
                {
                    if (cartItem.Item != null)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = Order.Id,
                            ItemId = cartItem.ItemId,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Item.Price
                        };
                        _context.OrderItems.Add(orderItem);

                        // Update inventory
                        cartItem.Item.QuantityRemaining -= cartItem.Quantity;
                        _context.Update(cartItem.Item);
                    }
                }

                _context.CartItems.RemoveRange(Items);
                await _context.SaveChangesAsync();

                // Set order success flag before clearing session data
                TempData["OrderSuccess"] = true;

                // Clear guest session data if this was a guest order
                if (guestCartId != null)
                {
                    HttpContext.Session.Remove("GuestCartId");
                    HttpContext.Session.Remove("GuestEmail");
                    HttpContext.Session.Remove("CheckoutAsGuest");
                }

                return RedirectToPage("/OrderConfirmation", new { orderId = Order.Id });
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error processing order");
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                return Page();
            }
        }
    }
} 