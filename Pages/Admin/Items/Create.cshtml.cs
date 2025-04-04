using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext db, IWebHostEnvironment environment, ILogger<CreateModel> logger)
        {
            _db = db;
            _environment = environment;
            _logger = logger;
        }

        [BindProperty]
        public Item Item { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Load categories for the dropdown
            Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Reload categories in case we need to return to the page
                Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");

                // Clear any existing validation errors for the Category property
                ModelState.Remove("Item.Category");

                // Validate CategoryId
                if (Item.CategoryId <= 0)
                {
                    ModelState.AddModelError("Item.CategoryId", "Please select a category");
                    return Page();
                }

                // Get the category and set it on the item before validation
                var category = await _db.Categories.FindAsync(Item.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("Item.CategoryId", "Selected category does not exist");
                    return Page();
                }

                // Set both CategoryId and Category properties before validation
                Item.CategoryId = category.Id;
                Item.Category = category;

                // Clear any existing validation errors for the Category property again
                ModelState.Remove("Item.Category");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                        }
                    }
                    return Page();
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Validate file size (5MB max)
                    if (ImageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "File size cannot exceed 5MB.");
                        return Page();
                    }

                    // Validate file type
                    var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                    
                    if (!allowedTypes.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Only image files (jpg, jpeg, png, gif) are allowed.");
                        return Page();
                    }

                    try
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "items");
                        Directory.CreateDirectory(uploadsFolder);

                        // Generate unique filename
                        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // Set the ImageUrl to the relative path
                        Item.ImageUrl = $"/uploads/items/{uniqueFileName}";
                        _logger.LogInformation($"Successfully saved image to {filePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving image file");
                        ModelState.AddModelError("ImageFile", "Error saving image file. Please try again.");
                        return Page();
                    }
                }
                else
                {
                    // Set default image if no file was uploaded
                    Item.ImageUrl = "/images/default-item.png";
                    _logger.LogInformation("Using default image");
                }

                try
                {
                    _logger.LogInformation($"Adding new item: {Item.Name}, Category: {Item.CategoryId}, Price: {Item.Price}");
                    _db.Items.Add(Item);
                    await _db.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Item '{Item.Name}' was successfully created.";
                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving item to database");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the item. Please try again.");
                    
                    // If we uploaded an image but failed to save to DB, try to clean it up
                    if (ImageFile != null && !string.IsNullOrEmpty(Item.ImageUrl))
                    {
                        try
                        {
                            var filePath = Path.Combine(_environment.WebRootPath, Item.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                                _logger.LogInformation($"Cleaned up image file after failed item creation: {filePath}");
                            }
                        }
                        catch (Exception cleanupEx)
                        {
                            _logger.LogError(cleanupEx, "Error cleaning up image file after failed item creation");
                        }
                    }
                    
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in OnPostAsync");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return Page();
            }
        }
    }
}
