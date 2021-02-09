using System;
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


namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaceRecognitionController : ControllerBase
    {
        private readonly ILogger<FaceRecognitionController> _logger;
        private readonly ICognitiveVisionService _cognitivevision;
        private readonly  IFirebaseService _firebase;
        public FaceRecognitionController(ILogger<FaceRecognitionController> logger,IFirebaseService firebase, ICognitiveVisionService cognitivevision)
        {
            _logger = logger;
            _cognitivevision = cognitivevision;
            _firebase = firebase;
        }

        [HttpPost]
        //[Authorize]
        public  async Task<IActionResult> FaceDetect (string IMAGE_URL)
        {
            try
            {
                List<DetectedFace> facesDetected = await _cognitivevision.DetectFaceRecognize( IMAGE_URL, RecognitionModel.Recognition03);
                Dictionary<string, object> dataObject = _firebase.TransformObjectRecognition(IMAGE_URL,facesDetected[0].FaceId.Value.ToString());
                bool save = await _firebase.SaveObject("face-reconigtion",dataObject);
                return new ObjectResult(new {  facesDetected , facesCount = facesDetected.Count }) { StatusCode = 200};
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message , facesCount = 0 }) { StatusCode = 500};
            }
            
        }

        

        
    }
}
