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
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Google.Cloud.Firestore;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaceCompareByUrlsController : ControllerBase
    {
        private readonly ILogger<FaceCompareByUrlsController> _logger;
        private readonly ICognitiveVisionService _cognitivevision;
        private readonly  IFirebaseService _firebase;

        public FaceCompareByUrlsController(ILogger<FaceCompareByUrlsController> logger, IFirebaseService firebase,ICognitiveVisionService cognitivevision)
        {
            _logger = logger;
            _cognitivevision = cognitivevision;
            _firebase = firebase;
        }

        [HttpPost]
        //[Authorize]
        public  async Task<IActionResult> FaceCompare(string IMAGE_URL1, string IMAGE_URL2)
        {
            try
            {
                bool saveFirebase;

                List<DetectedFace> facesDetected1 = await _cognitivevision.DetectFaceRecognize(IMAGE_URL1, RecognitionModel.Recognition03);
                Dictionary<string, object> dataObject1 = _firebase.TransformObjectRecognition(IMAGE_URL1,facesDetected1[0].FaceId.Value.ToString());
                saveFirebase = await _firebase.SaveObject("face-reconigtion",dataObject1);
                if(facesDetected1.Count != 1 ){ throw new ArgumentException("Una de las fotos contiene ninguna o mas de una cara."); }
                

                List<DetectedFace> facesDetected2 = await _cognitivevision.DetectFaceRecognize(IMAGE_URL2, RecognitionModel.Recognition03);
                Dictionary<string, object> dataObject2 = _firebase.TransformObjectRecognition(IMAGE_URL2,facesDetected2[0].FaceId.Value.ToString());
                saveFirebase = await _firebase.SaveObject("face-reconigtion",dataObject2);
                if(facesDetected2.Count != 1 ){ throw new ArgumentException("Una de las fotos contiene ninguna o mas de una cara."); }

                Guid guid1 = facesDetected1[0].FaceId.Value;
                Guid guid2 = facesDetected2[0].FaceId.Value;

                VerifyResult verifyObjectResult = await _cognitivevision.VerifyTwoFaces( guid1,  guid2);
                Dictionary<string, object> dataObject3 = _firebase.TransformObjectCompare(verifyObjectResult, guid1,  guid2);
                saveFirebase = await _firebase.SaveObject("face-compare",dataObject3);
                return new ObjectResult(new {IsIdentical = verifyObjectResult.IsIdentical , Confidence = verifyObjectResult.Confidence }) { StatusCode = 200};
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message , facesCount = 0 }) { StatusCode = 500};
            }
        }

        
    }
}
