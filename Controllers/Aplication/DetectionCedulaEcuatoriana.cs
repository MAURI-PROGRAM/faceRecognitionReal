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
using Newtonsoft.Json;


using System.Net.Http;
using System.Net.Http.Headers;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DetectionCedulaEcuatoriana : ControllerBase
    {
        private readonly ILogger<ImageUploadController> _logger;
        private readonly  IFirebaseService _firebase;
        private readonly  ICustomVisionService _customvision;


        /// <inheritdoc />
        /// <summary>
        /// Image Upload Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="firebase"></param>
        public DetectionCedulaEcuatoriana(ILogger<ImageUploadController> logger, IFirebaseService firebase,ICustomVisionService customvision)
        {
            _logger = logger;
            _firebase = firebase;
            _customvision = customvision;
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
                    HttpResponseMessage response;
                    response = await _customvision.IdentifyCedula(filePath);
                    
                    string responsecontent = await response.Content.ReadAsStringAsync();
                    DetectionEntity data = JsonConvert.DeserializeObject<DetectionEntity>(responsecontent);              
                    Dictionary<string, object> dataObject = _firebase.TransformObjectDetection(JsonConvert.SerializeObject(data.id),JsonConvert.SerializeObject(data.predictions));
                    bool saveFirebase = await _firebase.SaveObject("detection-cedula",dataObject);
                    return new ObjectResult(data) { StatusCode = 200};
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
