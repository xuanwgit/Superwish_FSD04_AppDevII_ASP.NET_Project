using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class ItemDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ItemDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Item? Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (Item == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int itemId, int quantity)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            if (item.QuantityRemaining < quantity)
            {
                TempData["ErrorMessage"] = $"Sorry, only {item.QuantityRemaining} items are available.";
                return RedirectToPage(new { id = itemId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            if (userId == null && guestCartId == null)
            {
                // Generate a new guest cart ID if neither exists
                guestCartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("GuestCartId", guestCartId);
            }

            // Check if the item already exists in the cart
            CartItem? cartItem;
            if (userId != null)
            {
                cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId);
            }
            else
            {
                cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(c => c.GuestCartId == guestCartId && c.ItemId == itemId);
            }

            if (cartItem != null)
            {
                // Update existing cart item
                if (cartItem.Quantity + quantity > item.QuantityRemaining)
                {
                    TempData["ErrorMessage"] = $"Cannot add {quantity} more items. Only {item.QuantityRemaining - cartItem.Quantity} more available.";
                    return RedirectToPage(new { id = itemId });
                }
                cartItem.Quantity += quantity;
                _context.Update(cartItem);
            }
            else
            {
                // Create new cart item
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
            TempData["SuccessMessage"] = $"{quantity} item(s) added to cart successfully.";
            return RedirectToPage("/Cart");
        }
    }
} 