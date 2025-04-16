using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ItemsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Item> Items { get; set; } = new List<Item>();
        public string? CategoryName { get; set; }

        public async Task OnGetAsync(string? category)
        {
            var query = _db.Items
                .Include(i => i.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category.Name == category);
                CategoryName = category;
            }

            Items = await query.ToListAsync();
        }
    }
} 