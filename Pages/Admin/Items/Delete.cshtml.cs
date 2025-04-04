using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Item Item { get; set; } = default!;
        public bool HasOrders { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _db.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            Item = item;
            HasOrders = await _db.OrderItems.AnyAsync(oi => oi.ItemId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                _db.Items.Remove(item);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Item '{item.Name}' was successfully deleted.";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException)
            {
                // If we get here, something went wrong with the delete
                ModelState.AddModelError(string.Empty, 
                    "Unable to delete this item. It may be referenced by orders or other records.");
                return Page();
            }
        }
    }
}
