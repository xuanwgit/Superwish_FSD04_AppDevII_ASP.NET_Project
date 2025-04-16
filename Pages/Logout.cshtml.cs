using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger) {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (signInManager.IsSignedIn(User) && User.Identity?.Name != null) {
                logger.LogInformation($"User {User.Identity.Name} logged out");
            }
            
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Sign out the user
            await signInManager.SignOutAsync();
            
            // Redirect to home page with a message
            TempData["Message"] = "You have been successfully logged out.";
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (signInManager.IsSignedIn(User) && User.Identity?.Name != null) {
                logger.LogInformation($"User {User.Identity.Name} logged out");
            }
            
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Sign out the user
            await signInManager.SignOutAsync();
            
            // Redirect to home page with a message
            TempData["Message"] = "You have been successfully logged out.";
            return RedirectToPage("/Index");
        }
    }
}
