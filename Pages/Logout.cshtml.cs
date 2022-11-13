using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> logger;


        private readonly SignInManager<IdentityUser> _signInManager;
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            this._signInManager = signInManager;
            this.logger = logger;
        }
        public async  void OnGet()
        {
            LoginModel.Login = "Login";
            await _signInManager.SignOutAsync();
        }
    }
}
