using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Settings
{
    [Authorize(Roles = "Admin")]
    public class BackgroundModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BackgroundModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public string? CurrentBackgroundUrl { get; set; }

        public void OnGet()
        {
            var wwwrootPath = _webHostEnvironment.WebRootPath;
            var backgroundPath = Path.Combine(wwwrootPath, "images", "backgrounds", "hero-bg.jpg");
            
            if (System.IO.File.Exists(backgroundPath))
            {
                CurrentBackgroundUrl = "/images/backgrounds/hero-bg.jpg";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var backgroundImage = Request.Form.Files["BackgroundImage"];
            if (backgroundImage == null || backgroundImage.Length == 0)
            {
                ModelState.AddModelError("", "Please select an image file.");
                return Page();
            }

            var wwwrootPath = _webHostEnvironment.WebRootPath;
            var backgroundsPath = Path.Combine(wwwrootPath, "images", "backgrounds");

            // Create the backgrounds directory if it doesn't exist
            if (!Directory.Exists(backgroundsPath))
            {
                Directory.CreateDirectory(backgroundsPath);
            }

            var filePath = Path.Combine(backgroundsPath, "hero-bg.jpg");

            // Delete existing file if it exists
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Save the new file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await backgroundImage.CopyToAsync(fileStream);
            }

            TempData["Message"] = "Background image updated successfully.";
            return RedirectToPage();
        }
    }
} 