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
    public class IndexModel : PageModel
    {
        private readonly RazorPagesToy.Data.Fsd04toysdbContext _context;

        public IndexModel(RazorPagesToy.Data.Fsd04toysdbContext context)
        {
            _context = context;
        }

        public IList<Toy> Toy { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Toy != null)
            {
                Toy = await _context.Toy.ToListAsync();
            }
        }
    }
}
