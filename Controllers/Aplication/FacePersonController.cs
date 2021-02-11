using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Api_face_recognition.Domain;
using Microsoft.Extensions.Options;
using Api_face_recognition.Services;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacePersonController : ControllerBase
    {
        private readonly ILogger<FacePersonController> _logger;
        private readonly AzureStorageConfiguration _azureStorage;
        private readonly ICognitiveVisionService _cognitivevision;
        private readonly  IFirebaseService _firebase;
        public FacePersonController(ILogger<FacePersonController> logger,IFirebaseService firebase, ICognitiveVisionService cognitivevision, IOptions<AzureStorageConfiguration> azureStorage)
        {
            _logger = logger;
            _cognitivevision = cognitivevision;
            _azureStorage = azureStorage.Value ?? throw new ArgumentNullException(nameof(azureStorage));
            _firebase = firebase;
        }

        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> FacePerson (List<IFormFile> files, Double umbral)
        {
            try{
                if(files.Count> 1 & umbral > 0 &  umbral < 0.5 ){
                    int isEyesBlink = 0;
                    int notEyesBlink = 0;
                    Boolean EyeBlink = true;
                    Boolean CapturePhoto = false;
                    string urlPhotos = "deteccion de parpadeo"  ;
                    
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0){

                            var filePath = Path.GetTempFileName();

                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            using (Stream imageFileStream = System.IO.File.OpenRead(filePath))
                            {
                                List<DetectedFace> facesDetected = await _cognitivevision.DetectFaceRecognizeStream(imageFileStream, RecognitionModel.Recognition03);
                                if(facesDetected.Count != 1 ){ throw new ArgumentException("Una de las fotos contiene ninguna o mas de una cara."); }
                                EyeBlink = _cognitivevision.EyesBlink(facesDetected, umbral);
                                isEyesBlink = EyeBlink?isEyesBlink+1:isEyesBlink;
                                notEyesBlink = !EyeBlink?notEyesBlink+1:notEyesBlink;

                                if( !CapturePhoto && !EyeBlink){
                                    BlobContainerClient container = new BlobContainerClient(_azureStorage.ConnectionString, _azureStorage.ContainerName);
                                    string fileName = DateTime.Now.ToString("MMddyyyyHHmmssff") + formFile.FileName;
                                    BlobClient blob = container.GetBlobClient(fileName);
                                    blob.Upload(filePath);
                                    Uri webazurestorage = new Uri(_azureStorage.UrlStorage );
                                    Uri urlPhoto = new Uri(webazurestorage, _azureStorage.ContainerName + "/" +fileName);
                                    urlPhotos = urlPhoto.ToString();
                                    Dictionary<string, object> dataObject = _firebase.TransformObjectImageUpload( urlPhotos);
                                    bool save = await _firebase.SaveObject("image-upload",dataObject);                                
                                };
                                Dictionary<string, object> dataObject1 = _firebase.TransformObjectRecognition( urlPhotos , facesDetected[0].FaceId.Value.ToString());
                                bool saveFirebase = await _firebase.SaveObject("face-reconigtion",dataObject1);
                            }
                        } else{
                            throw new ArgumentException("Error al parpadear");
                        }
                    }
                    Boolean HayPersona = isEyesBlink>0 && notEyesBlink>0;
                    
                    return new ObjectResult(new { HayPersona , urlPhotos }) { StatusCode = 200};
                }else{
                    throw new ArgumentException("Se necesita almenos dos fotos para realizar este proceso, el umbral debe ser mayor a 0 y menor a 0.5");
                }
                
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message  }) { StatusCode = 500};
            }
        }
        
    }
}
