using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class CartModel : PageModel
    {

        private readonly ILogger<CartModel> logger;
        
        private readonly ToysDbContext db;

        private readonly UserManager<IdentityUser> _userManager;

        public CartModel(ToysDbContext db,ILogger<CartModel> logger1, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            logger = logger1;
            _userManager = userManager;
        }

        [BindProperty]
        public Cart_Item cartItem { get; set; }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Item item { get; set; }

        public async Task OnGetAsync()
        {
            item = db.Items.Find(Id);
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            cartItem.userId = user;
            cartItem.itemId = item;
            cartItem.quantity = 1;
        }
        
    }
}
