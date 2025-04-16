using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Footer
{
    public class ContactUsModel : PageModel
    {
        [BindProperty]
        public ContactForm Contact { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Process the contact form (e.g., send email, save to database)
            TempData["Message"] = "Thank you for your message. We'll get back to you soon!";
            return RedirectToPage();
        }
    }

    public class ContactForm
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
} 