using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesToy.Data;
using RazorPagesToy.Models;

namespace RazorPagesToy.Pages.Toys
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesToy.Data.Fsd04toysdbContext _context;

        public CreateModel(RazorPagesToy.Data.Fsd04toysdbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Toy Toy { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Toy.Add(Toy);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
