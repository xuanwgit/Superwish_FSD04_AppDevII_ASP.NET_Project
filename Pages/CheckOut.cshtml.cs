using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class CheckOutModel : PageModel
    {
        private static object lck = new object();
        private readonly ILogger<CartModel> logger;

        private readonly ToysDbContext db;

        private readonly UserManager<IdentityUser> _userManager;
        public CheckOutModel(ToysDbContext db, ILogger<CartModel> logger1, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            logger = logger1;
            _userManager = userManager;
        }

        public Cart_Item[] Items { get; set; }
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
            switch(action)
            {
                case "checkout":
                    {
                        lock (lck)
                        {
                            var shops = db.Items.ToArray();
                            var citems = db.Cart_Items?.Where(x => x.userId.Id == user.Id).ToArray();
                            if (citems != null)
                            {
                                List<Cart_Item> finishItem = new List<Cart_Item>();
                                foreach (var citem in citems)
                                {
                                    
                                        var shop = shops.Where(x => x.Id == citem.itemId.Id).FirstOrDefault();
                                        if (shop == null) continue;
                                        if (citem.quantity > shop.QuantityRemaining) continue;
                                        OrderItem orderItem = new OrderItem()
                                        {
                                            item_id = shop,
                                            price = (int)shop.Price,
                                            quantity = citem.quantity
                                        };
                                        db.OrderItems.Add(orderItem);
                                        shop.QuantityRemaining -= citem.quantity;
                                        finishItem.Add(citem);
                                    
                                }
                                if (finishItem.Count > 0)
                                {
                                    db.Cart_Items.RemoveRange(finishItem);
                                }
                                db.SaveChangesAsync();
                                ViewData["tip"] = "thanks for your purchase";
                                return;
                            }
                        }
                        break;
                    }
                default:
                    {
                        lock (lck)
                        {
                            try
                            {

                                var shops = db.Items?.ToArray();

                                var citems = db.Cart_Items?.Where(x => x.userId.Id == user.Id).ToArray();
                                if (citems != null && null!= shops)
                                {
                                    List<Cart_Item> finishItem = new List<Cart_Item>();
                                    Dictionary<int, int> shop_itemDic = new Dictionary<int, int>();
                                    foreach (var item in shops)
                                    {
                                        shop_itemDic.Add(item.Id, item.QuantityRemaining);
                                    }
                                    foreach (var citem in citems)
                                    {

                                        var shop = shops.Where(x => x.Id == citem.itemId.Id).FirstOrDefault();
                                        if (shop == null) continue;
                                        if (citem.quantity > shop_itemDic[shop.Id]) continue;

                                        shop_itemDic[shop.Id] -= citem.quantity;
                                        finishItem.Add(citem);

                                    }
                                    Items = finishItem.ToArray();

                                    ViewData["tip"] = "pay info:";
                                    return;
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        break;
                    }
            }
           
        }
    }
}
