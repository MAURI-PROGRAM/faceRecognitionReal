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


using System.Net.Http;
using System.Net.Http.Headers;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DetectionCedulaEcuatoriana : ControllerBase
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
        public DetectionCedulaEcuatoriana(ILogger<ImageUploadController> logger, IFirebaseService firebase,IOptions<AzureStorageConfiguration> azureStorage)
        {
            _logger = logger;
            _azureStorage = azureStorage.Value ?? throw new ArgumentNullException(nameof(azureStorage));
            _firebase = firebase;
        }

        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> OnPostUploadAsync(IFormFile formFile)
        {

            try
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Prediction-Key", "8b7f629ac9124bf89197978abaafeba5");
                    string url = "https://ocrdatoscedula.cognitiveservices.azure.com/customvision/v3.0/Prediction/349138c1-b376-41bf-8bb6-4fc6716a7743/classify/iterations/Iteration2/image";
                    HttpResponseMessage response;
                    byte[] byteData = GetImageAsByteArray(filePath);

                    using (var content = new ByteArrayContent(byteData))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(url, content);
                        return new ObjectResult(await response.Content.ReadAsStringAsync()) { StatusCode = 200};
                        //Console.WriteLine();
                    }

                    
                } else{

                    return new ObjectResult(new { error = "No se encontro ninguna imagen tipo" }) { StatusCode = 500};
                }
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message }) { StatusCode = 500};
            }
        }


        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        
    }
}
