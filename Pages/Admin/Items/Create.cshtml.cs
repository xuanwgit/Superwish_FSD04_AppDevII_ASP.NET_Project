using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
        [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext _context;

        public CreateModel(Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Item Item { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Items.Add(Item);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
