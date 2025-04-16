using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class FeaturedModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public FeaturedModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Item> FeaturedItems { get; set; } = new List<Item>();

        public async Task OnGetAsync()
        {
            // Get all items with their categories, ordered by newest first
            // For now, we'll show all items as featured
            FeaturedItems = await _db.Items
                .Include(i => i.Category)
                .OrderByDescending(i => i.Id) // Assuming Id is auto-incrementing
                .ToListAsync();
        }
    }
} 