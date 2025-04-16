using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Categories
                .Include(c => c.Items)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            // Check if any items in this category are referenced by orders
            var hasOrderedItems = await _context.OrderItems
                .AnyAsync(oi => category.Items.Any(i => i.Id == oi.ItemId));

            if (hasOrderedItems)
            {
                ErrorMessage = "Cannot delete this category because it contains items that have been ordered. Please delete the associated orders first.";
                return RedirectToPage();
            }

            // Check if category has any items
            if (category.Items.Any())
            {
                ErrorMessage = "Cannot delete this category because it contains items. Please delete or move the items first.";
                return RedirectToPage();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            SuccessMessage = "Category deleted successfully.";
            return RedirectToPage();
        }
    }
} 