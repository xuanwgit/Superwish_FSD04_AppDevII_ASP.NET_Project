using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class LoginModel : PageModel
    {
private readonly SignInManager<IdentityUser> signInManager;


        private readonly ILogger<RegisterModel> logger;

        public LoginModel (SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger) {
                this.signInManager = signInManager;
                this.logger = logger;
        }

        [BindProperty]
        public InputModel Input{ get; set; }

        public class InputModel {
            [Required]
            [EmailAddress]
            public string Email { get; set;}

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }


       public async Task<IActionResult> OnPostAsync() {
                if(ModelState.IsValid){
                    var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, true);
                    if (result.Succeeded){
                        logger.LogInformation($"User {Input.Email} Logged In");
                        return RedirectToPage("Index");
                    }
                    else{
                        ModelState.AddModelError(string.Empty, "Login failed");
                    }
                }
                return Page();
        }

         public void OnGet()
        {
        }
    }
}

