using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [BindProperty]
        public Item Item { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _db.Items.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            Item = item;
            Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
                return Page();
            }

            var existingItem = await _db.Items.FindAsync(Item.Id);
            if (existingItem == null)
            {
                return NotFound();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Validate file type
                var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                
                if (!allowedTypes.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (jpg, jpeg, png, gif) are allowed.");
                    Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
                    return Page();
                }

                // Delete old image if it exists and is not the default image
                if (!string.IsNullOrEmpty(existingItem.ImageUrl) && !existingItem.ImageUrl.StartsWith("/images/default"))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, existingItem.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "items");
                
                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Update the ImageUrl
                Item.ImageUrl = $"/uploads/items/{uniqueFileName}";
            }
            else
            {
                // Keep the existing image URL if no new image is uploaded
                Item.ImageUrl = existingItem.ImageUrl;
            }

            // Update other properties
            existingItem.Name = Item.Name;
            existingItem.Description = Item.Description;
            existingItem.Price = Item.Price;
            existingItem.QuantityRemaining = Item.QuantityRemaining;
            existingItem.CategoryId = Item.CategoryId;
            existingItem.ImageUrl = Item.ImageUrl;

            try
            {
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Item '{Item.Name}' was successfully updated.";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ItemExists(int id)
        {
            return _db.Items.Any(e => e.Id == id);
        }
    }
}
