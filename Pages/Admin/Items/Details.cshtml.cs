using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext db, ILogger<DetailsModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Item Item { get; set; } = default!;
        public List<OrderItem> OrderItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include Category for the item
            Item = await _db.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Item == null)
            {
                return NotFound();
            }

            // Get order history for this item
            OrderItems = await _db.OrderItems
                .Include(oi => oi.Order)
                .Where(oi => oi.ItemId == id)
                .OrderByDescending(oi => oi.Order.OrderDate)
                .ToListAsync();

            return Page();
        }
    }
}
