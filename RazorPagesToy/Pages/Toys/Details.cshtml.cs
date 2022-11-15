using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesToy.Data;
using RazorPagesToy.Models;

namespace RazorPagesToy.Pages.Toys
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPagesToy.Data.Fsd04toysdbContext _context;

        public DetailsModel(RazorPagesToy.Data.Fsd04toysdbContext context)
        {
            _context = context;
        }

      public Toy Toy { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Toy == null)
            {
                return NotFound();
            }

            var toy = await _context.Toy.FirstOrDefaultAsync(m => m.ID == id);
            if (toy == null)
            {
                return NotFound();
            }
            else 
            {
                Toy = toy;
            }
            return Page();
        }
    }
}
