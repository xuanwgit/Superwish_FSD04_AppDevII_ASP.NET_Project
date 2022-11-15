using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Helpers;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
    public class ImagesModel : PageModel
    {
       /* BlobContainerClient _containerClient;
        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }*/
        public void OnGet()
        {
        }
    }
}
