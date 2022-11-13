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

    private readonly ILogger<RegisterModel> logger;
    
    private readonly ToysDbContext db;

    public IndexModel(ToysDbContext db,ILogger<IndexModel> logger1)
    {
        this.db = db;
        _logger = logger1;
    }

    public List<Item> Items { get; set; } = new List<Item>();  

    public Item FeaturedItem { get; set; } 
    
    public async Task OnGetAsync()
    {
        Items = await db.Items.ToListAsync();
        //FeaturedItem=Items.First();
        FeaturedItem = Items.ElementAt(new Random().Next(Items.Count));   
    }    
}
        
