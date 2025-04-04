using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Data;
using System.Transactions;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly ILogger<CartModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CheckOutModel(ApplicationDbContext db, ILogger<CartModel> logger, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        public CartItem[]? Items { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string Action { get; set; } = string.Empty;
       
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                Response.Redirect("Login");
                return;
            }

            switch(Action)
            {
                case "checkout":
                    await ProcessCheckoutAsync(user);
                    break;
                default:
                    await LoadCartItemsAsync(user);
                    break;
            }
        }

        private async Task ProcessCheckoutAsync(IdentityUser user)
        {
            // Start a database transaction with explicit System.Data.IsolationLevel
            using var transaction = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                // Lock the items we're going to modify using FOR UPDATE in PostgreSQL
                var cartItems = await _db.CartItems
                    .Where(x => x.UserId == user.Id)
                    .ToListAsync();

                if (!cartItems.Any())
                {
                    return;
                }

                // Get all item IDs from cart
                var itemIds = cartItems.Select(x => x.ItemId).ToList();

                // Lock and load items with FOR UPDATE to prevent concurrent modifications
                var shops = await _db.Items
                    .FromSqlRaw("SELECT * FROM \"Items\" WHERE \"Id\" = ANY({0}) FOR UPDATE", itemIds)
                    .ToListAsync();

                var finishItems = new List<CartItem>();
                var order = new Order
                {
                    UserId = user.Id,
                    Total = 0,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    // Initialize required fields with empty strings to prevent null constraint violations
                    AddressLine1 = string.Empty,
                    City = string.Empty,
                    StateProvince = string.Empty,
                    PostalCode = string.Empty,
                    Country = string.Empty,
                    CardNumber = string.Empty,
                    CardHolderName = string.Empty,
                    CVV = string.Empty
                };

                _db.Orders.Add(order);

                foreach (var cartItem in cartItems)
                {
                    var shop = shops.FirstOrDefault(x => x.Id == cartItem.ItemId);
                    if (shop == null || cartItem.Quantity > shop.QuantityRemaining)
                    {
                        // Rollback transaction if inventory check fails
                        await transaction.RollbackAsync();
                        ViewData["tip"] = "Some items are no longer available in the requested quantity";
                        return;
                    }

                    var orderItem = new OrderItem
                    {
                        Item = shop,
                        Price = shop.Price,
                        Quantity = cartItem.Quantity,
                        Order = order
                    };

                    order.Total += shop.Price * cartItem.Quantity;
                    shop.QuantityRemaining -= cartItem.Quantity;
                    
                    _db.OrderItems.Add(orderItem);
                    finishItems.Add(cartItem);
                }

                if (finishItems.Any())
                {
                    _db.CartItems.RemoveRange(finishItems);
                    await _db.SaveChangesAsync();
                    
                    // Commit transaction only if everything succeeded
                    await transaction.CommitAsync();
                    ViewData["tip"] = "Thanks for your purchase!";
                }
            }
            catch (Exception ex)
            {
                // Rollback transaction on any error
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing checkout for user {UserId}", user.Id);
                ViewData["tip"] = "An error occurred during checkout. Please try again.";
            }
        }

        private async Task LoadCartItemsAsync(IdentityUser user)
        {
            try
            {
                // Use explicit System.Data.IsolationLevel
                using var transaction = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

                var cartItems = await _db.CartItems
                    .Include(x => x.Item) // Eager load items
                    .Where(x => x.UserId == user.Id)
                    .ToListAsync();

                if (!cartItems.Any())
                {
                    return;
                }

                // Get all items in a single query with current inventory
                var itemIds = cartItems.Select(x => x.ItemId).ToList();
                var shops = await _db.Items
                    .Where(x => itemIds.Contains(x.Id))
                    .ToListAsync();

                var shopInventory = shops.ToDictionary(x => x.Id, x => x.QuantityRemaining);
                var validItems = new List<CartItem>();

                foreach (var cartItem in cartItems)
                {
                    if (shopInventory.TryGetValue(cartItem.ItemId, out var remaining) && 
                        cartItem.Quantity <= remaining)
                    {
                        validItems.Add(cartItem);
                    }
                }

                Items = validItems.ToArray();
                ViewData["tip"] = validItems.Any() ? "Ready for checkout!" : "No items available for checkout";

                // Commit read-only transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart items for user {UserId}", user.Id);
                ViewData["tip"] = "An error occurred loading your cart. Please try again.";
            }
        }
    }
}
