using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Helpers;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Pages
{
	public class UploadModel : PageModel
	{

		BlobContainerClient _containerClient;


		public UploadModel(IOptions<AzureStorageConfig> config, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				_containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=toyblob;AccountKey=g5oN/2fdezyuFzzaMUYg+0r/lk3GPYvdMDd6Y3DGxIAqIGryrI7miqB7sl7tISgPJBN5IcBcszK5+ASt3MKX8A==;EndpointSuffix=core.windows.net", "toyblobcontainer");
			}
			else
			{
				string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
														config.Value.AccountName,
													   config.Value.ContainerName);

				_containerClient = new BlobContainerClient(new Uri("DefaultEndpointsProtocol=https;AccountName=toyblob;AccountKey=g5oN/2fdezyuFzzaMUYg+0r/lk3GPYvdMDd6Y3DGxIAqIGryrI7miqB7sl7tISgPJBN5IcBcszK5+ASt3MKX8A==;EndpointSuffix=core.windows.net"),
																			new DefaultAzureCredential());
			}
		}

		[BindProperty]
		public IFormFile Upload { get; set; }




		public async Task<IActionResult> OnPostAsync()
		{

			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				// Create the container if it does not exist.
				await _containerClient.CreateIfNotExistsAsync();

				// Upload the file to the container
				await _containerClient.UploadBlobAsync(Upload.FileName, Upload.OpenReadStream());
			}
			catch (Exception e)
			{
				throw e;
			}



			return Page();

		}/* static void ImageUrl()
{
    var account = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
    var cloudBlobClient = account.CreateCloudBlobClient();
    var container = cloudBlobClient.GetContainerReference("container-name");
    var blob = container.GetBlockBlobReference("image.png");
    blob.UploadFromFile("File Path ....");//Upload file....

    var ImnageUrl = blob.Uri.AbsoluteUri;
}*/
	}
}