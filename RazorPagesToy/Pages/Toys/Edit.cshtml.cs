using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesToy.Data;
using RazorPagesToy.Models;

namespace RazorPagesToy.Pages.Toys
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesToy.Data.Fsd04toysdbContext _context;

        public EditModel(RazorPagesToy.Data.Fsd04toysdbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Toy Toy { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Toy == null)
            {
                return NotFound();
            }

            var toy =  await _context.Toy.FirstOrDefaultAsync(m => m.ID == id);
            if (toy == null)
            {
                return NotFound();
            }
            Toy = toy;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Toy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToyExists(Toy.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ToyExists(int id)
        {
          return _context.Toy.Any(e => e.ID == id);
        }
    }
}
