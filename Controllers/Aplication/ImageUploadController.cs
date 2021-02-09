using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Api_face_recognition.Domain;
using Microsoft.Extensions.Options;
using Api_face_recognition.Services;
using System.IO;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ILogger<ImageUploadController> _logger;
        private readonly AzureStorageConfiguration _azureStorage;
        private readonly  IFirebaseService _firebase;


        /// <inheritdoc />
        /// <summary>
        /// Image Upload Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="firebase"></param>
        /// <param name="azureStorage"></param>
        public ImageUploadController(ILogger<ImageUploadController> logger, IFirebaseService firebase,IOptions<AzureStorageConfiguration> azureStorage)
        {
            _logger = logger;
            _azureStorage = azureStorage.Value ?? throw new ArgumentNullException(nameof(azureStorage));
            _firebase = firebase;
        }
        [HttpPost]
        //[Authorize]
        public  async Task<IActionResult> OnPostUploadAsync(IFormFile formFile)
        {

            try
            {
                BlobContainerClient container = new BlobContainerClient(_azureStorage.ConnectionString, _azureStorage.ContainerName);
                string fileName = DateTime.Now.ToString("MMddyyyyHHmmssff") + formFile.FileName;
                BlobClient blob = container.GetBlobClient(fileName);
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    blob.Upload(filePath);
                    Uri webazurestorage = new Uri(_azureStorage.UrlStorage );
                    Uri urlPhoto = new Uri(webazurestorage, _azureStorage.ContainerName + "/" +fileName);
                    Dictionary<string, object> dataObject = _firebase.TransformObjectImageUpload( urlPhoto.ToString());
                    bool save = await _firebase.SaveObject("image-upload",dataObject);
                    return new ObjectResult(new { urlPhoto}) { StatusCode = 200};
                } else{
                    return new ObjectResult(new { error = "No se encontro ninguna imagen tipo" }) { StatusCode = 500};
                }
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message }) { StatusCode = 500};
            }
        }

        
    }
}
