using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Account
{
    public class ManageModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ManageModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasPassword { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRegisteredUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Login");
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            Email = user.Email;
            PhoneNumber = await userManager.GetPhoneNumberAsync(user);
            HasPassword = await userManager.HasPasswordAsync(user);

            // Check user roles
            var roles = await userManager.GetRolesAsync(user);
            IsAdmin = roles.Contains("Admin");
            IsRegisteredUser = roles.Contains("User") || IsAdmin;

            return Page();
        }
    }
} 