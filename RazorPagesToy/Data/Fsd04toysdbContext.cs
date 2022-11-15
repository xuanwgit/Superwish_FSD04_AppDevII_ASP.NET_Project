using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesToy.Models;

namespace RazorPagesToy.Data
{
    public class Fsd04toysdbContext : DbContext
    {
        public Fsd04toysdbContext (DbContextOptions<Fsd04toysdbContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesToy.Models.Toy> Toy { get; set; } = default!;
    }
}
