using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartModel> _logger;

        public CartModel(ApplicationDbContext context, ILogger<CartModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<CartItem> Items { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            Items = await _context.CartItems
                .Include(c => c.Item)
                .Where(c => (userId != null && c.UserId == userId) || 
                          (guestCartId != null && c.GuestCartId == guestCartId))
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int itemId, string action, int? quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => 
                    ((userId != null && ci.UserId == userId) || 
                     (guestCartId != null && ci.GuestCartId == guestCartId)) && 
                    ci.ItemId == itemId);

            if (cartItem == null)
            {
                return RedirectToPage();
            }

            switch (action?.ToLower())
            {
                case "change" when quantity.HasValue:
                    cartItem.Quantity = quantity.Value;
                    break;
                case "delete":
                    _context.CartItems.Remove(cartItem);
                    break;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostContinueAsGuestAsync()
        {
            var guestCartId = HttpContext.Session.GetString("GuestCartId");
            
            // If no guest cart ID exists, create one
            if (string.IsNullOrEmpty(guestCartId))
            {
                guestCartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("GuestCartId", guestCartId);
            }

            // Check if cart is empty
            var cartItems = await _context.CartItems
                .Include(c => c.Item)
                .Where(c => c.GuestCartId == guestCartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add items before proceeding to checkout.";
                return RedirectToPage();
            }

            // Set checkout as guest flag
            HttpContext.Session.SetString("CheckoutAsGuest", "true");

            return RedirectToPage("/Order");
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int itemId, int quantity)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);
                if (item == null)
                {
                    return NotFound();
                }

                // Check if there's enough stock
                if (item.QuantityRemaining < quantity)
                {
                    TempData["ErrorMessage"] = $"Sorry, there are only {item.QuantityRemaining} units available for {item.Name}.";
                    return RedirectToPage();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guestCartId = HttpContext.Session.GetString("GuestCartId");

                // If no user ID or guest cart ID, create a new guest cart ID
                if (userId == null && guestCartId == null)
                {
                    guestCartId = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("GuestCartId", guestCartId);
                }

                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => 
                        ((userId != null && ci.UserId == userId) || 
                         (guestCartId != null && ci.GuestCartId == guestCartId)) && 
                        ci.ItemId == itemId);

                // Check if adding to existing cart item would exceed available stock
                if (cartItem != null)
                {
                    if (cartItem.Quantity + quantity > item.QuantityRemaining)
                    {
                        TempData["ErrorMessage"] = $"Sorry, there are only {item.QuantityRemaining} units available for {item.Name}. You already have {cartItem.Quantity} in your cart.";
                        return RedirectToPage();
                    }
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cartItem = new CartItem
                    {
                        ItemId = itemId,
                        Quantity = quantity,
                        UserId = userId,
                        GuestCartId = guestCartId
                    };
                    _context.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync();

                // Load updated cart items for the response
                Items = await _context.CartItems
                    .Include(c => c.Item)
                    .Where(c => (userId != null && c.UserId == userId) || 
                              (guestCartId != null && c.GuestCartId == guestCartId))
                    .ToListAsync();

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                TempData["ErrorMessage"] = "An error occurred while adding the item to your cart.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int itemId, string action)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            if (userId == null && guestCartId == null)
            {
                return RedirectToPage();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.Item)
                .FirstOrDefaultAsync(c => 
                    (userId != null && c.UserId == userId || 
                     guestCartId != null && c.GuestCartId == guestCartId) && 
                    c.ItemId == itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (action == "increase")
            {
                if (cartItem.Item != null && cartItem.Quantity < cartItem.Item.QuantityRemaining)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot increase quantity. Maximum stock limit reached.";
                }
            }
            else if (action == "decrease" && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            // If no user ID or guest cart ID, redirect to Cart page
            if (userId == null && guestCartId == null)
            {
                return RedirectToPage();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == itemId && 
                    ((userId != null && c.UserId == userId) || 
                     (guestCartId != null && c.GuestCartId == guestCartId)));

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}

