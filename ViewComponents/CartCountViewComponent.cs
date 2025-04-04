using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using System.Security.Claims;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CartCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var guestCartId = HttpContext.Session.GetString("GuestCartId");

            int totalItems = 0;

            if (userId != null)
            {
                totalItems = await _context.CartItems
                    .Where(c => c.UserId == userId)
                    .SumAsync(c => c.Quantity);
            }
            else if (guestCartId != null)
            {
                totalItems = await _context.CartItems
                    .Where(c => c.GuestCartId == guestCartId)
                    .SumAsync(c => c.Quantity);
            }

            return View(totalItems);
        }
    }
} 