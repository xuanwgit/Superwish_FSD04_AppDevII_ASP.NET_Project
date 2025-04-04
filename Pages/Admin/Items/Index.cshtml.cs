using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<Item> Items { get; set; } = default!;
        public string CurrentSort { get; set; } = string.Empty;
        public string NameSort { get; set; } = string.Empty;
        public string PriceSort { get; set; } = string.Empty;
        public string StockSort { get; set; } = string.Empty;

        public async Task OnGetAsync(string sortOrder)
        {
            // Set up sorting parameters
            CurrentSort = sortOrder;
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            PriceSort = sortOrder == "price" ? "price_desc" : "price";
            StockSort = sortOrder == "stock" ? "stock_desc" : "stock";

            // Start with all items and include Category
            var query = _db.Items.Include(i => i.Category).AsQueryable();

            // Apply sorting based on the sortOrder parameter
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(i => i.Name),
                "price" => query.OrderBy(i => i.Price),
                "price_desc" => query.OrderByDescending(i => i.Price),
                "stock" => query.OrderBy(i => i.QuantityRemaining),
                "stock_desc" => query.OrderByDescending(i => i.QuantityRemaining),
                _ => query.OrderBy(i => i.Name), // Default sort by name ascending
            };

            Items = await query.ToListAsync();
        }
    }
}
