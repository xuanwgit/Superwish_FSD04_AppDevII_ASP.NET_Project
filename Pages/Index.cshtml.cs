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
        //FeaturedItem = Items.ElementAt(new Random().Next(Items.Count));
    }    

// void IncreaseQty()  
//         {  
//             if (SD.quantity > 0)  
//             {  
//                 SD.quantity = SD.quantity + 1;  
//                 TotalAmount = ID.Price * SD.quantity;  

//             }  
//         }  

//         void DecreaseQty()  
//         {  
//             if (SD.quantity > 1)  
//             {  
//                 SD.quantity = SD.quantity - 1;  
//                 TotalAmount = ID.Price * SD.quantity;  
//             }  
//         } 

    // @functions {  
    //     string UserName = "Shanu";  
    //     string selectItemImage = "";  
    //     string selectedItemName = "";  
        
    //     decimal TotalAmount = 0;
    //     string Messages = "";  

    //     Cart_Item[] cart_Items;  
    //     Item[] items;  

    //     Cart_Item SD = new Cart_Item();  
    //     Item ID = new Item();  

    //     Boolean showAddtoCart = false;  

    //     void IncreaseQty()  
    //     {  
    //         if (SD.quantity > 0)  
    //         {  
    //             SD.quantity = SD.quantity + 1;  
    //             TotalAmount = ID.Price * SD.quantity;  

    //         }  
    //     }  

    //     void DecreaseQty()  
    //     {  
    //         if (SD.quantity > 1)  
    //         {  
    //             SD.quantity = SD.quantity - 1;  
    //             TotalAmount = ID.Price * SD.quantity;  
    //         }  
    //     } 

        
    // }
}
        
        
