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
        public Cart_Item[] Items { get; set; }
        public CartModel(ToysDbContext db,ILogger<CartModel> logger1, UserManager<IdentityUser> userManager)
        {
            if (cartItem == null) cartItem = new Cart_Item();
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
        [BindProperty(SupportsGet = true)]
        public int quantity { get; set; }
        [BindProperty(SupportsGet = true)]
        public string action { get; set; }
        public async Task OnGetAsync()
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {

                Response.Redirect("Login");
                return;
            }
            
                switch (action)
                {
                case "change":
                    {
                        var citem = db.Cart_Items.Find(Id);
                        if (citem != null)
                        {
                            citem.quantity = quantity;
                            db.SaveChanges();
                        }
                        break;

                    }
                    case "delete":
                        {
                            var citem = db.Cart_Items.Find(Id);
                            if(citem!=null)
                            {
                                db.Cart_Items.Remove(citem);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "Add to Cart":
                        {
                            item = db.Items.Find(Id);
                            if (item != null)
                            {
                                cartItem.Id = 0;
                                cartItem.userId = user;
                                cartItem.itemId = item;
                                cartItem.quantity = quantity;
                                db.Cart_Items.Add(cartItem);
                                var ret = db.SaveChanges();
                            }
                            break;
                        }
                }


            var shps = db.Items.ToArray();

            Items = db.Cart_Items.Where(x=>x.userId.Id==user.Id).ToArray();
          
        }
        
    }
}

