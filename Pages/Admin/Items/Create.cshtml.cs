using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Helpers;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages.Admin.Items
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext _context;

        BlobContainerClient _containerClient;

        public List<SelectListItem> Options { get; set; }

        public CreateModel(Superwish_FSD04_AppDevII_ASP.NET_Project.Data.ToysDbContext context)
        {
            _context = context;
        }

        /*public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try{
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {

                Options = _context.Items.Select(a =>
                                          new SelectListItem
                                          {
                                              Value = a.ImageUrl.ToString(),
                                          }).ToList();
            }
        }catch (Exception e)
            {
                throw e;
            }
            return Page();
        }*/
    
    /*Console.WriteLine("\t" + blobItem.Name);
    public List<SelectListItem> Options { get; set; }
    public void OnGet()
    {
        Options = _context.Items.Select(a =>
                                      new SelectListItem
                                      {
                                          Value = a.BlobItem.ToString(),
                                          Text = a.Name
                                      }).ToList();
    }*/

    public IActionResult OnGet()
    {
        return Page();
    }
    [BindProperty]
    public Category Category { get; set; }

    [BindProperty]
    public Item Item { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Items.Add(Item);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
}
