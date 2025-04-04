using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public List<Item> Items { get; set; } = new List<Item>();
    public List<Item> InStockItems { get; set; } = new List<Item>();
    public List<Item> OutOfStockItems { get; set; } = new List<Item>();
    public Item? FeaturedItem { get; set; }

    public async Task OnGetAsync()
    {
        // Get all items including their categories
        Items = await _db.Items.Include(i => i.Category).ToListAsync();
        
        // Separate items by stock status
        InStockItems = Items.Where(i => i.QuantityRemaining > 0).ToList();
        OutOfStockItems = Items.Where(i => i.QuantityRemaining <= 0).ToList();

        // Set featured item only from in-stock items
        if (InStockItems.Any())
        {
            FeaturedItem = InStockItems.ElementAt(new Random().Next(InStockItems.Count));
        }
    }
}
        
