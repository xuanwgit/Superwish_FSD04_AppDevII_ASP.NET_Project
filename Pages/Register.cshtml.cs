using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        private readonly ILogger<RegisterModel> logger;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterModel (UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger) {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.logger = logger;
        }

        public class InputModel {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} charcters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The Password and Confirmation do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() {
                if(ModelState.IsValid){
                    var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true};
                    var result = await userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded){
                        var result2 = await userManager.AddToRoleAsync(user,"User");
                        if (result2.Succeeded){
                            logger.LogInformation($"User {Input.Email} create a new account with password");
                        return RedirectToPage("Login", new { email = Input.Email });
                        }else{
                            //FIX ME
                        }
                        
                    }
                    foreach (var error in result.Errors){
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return Page();
        }
    }
}
