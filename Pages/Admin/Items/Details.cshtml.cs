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
        private readonly Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Item Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            // Force eager loading of Author information, otherwise it will be null
            Item = await _context.Items.Include(m => m.Price).FirstOrDefaultAsync(m => m.Id == id);

            if (Item == null)
            {
                return NotFound();
            }
            return Page();
        }
}
}
