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
using Google.Cloud.Firestore;

namespace Api_face_recognition.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaceCompareController : ControllerBase
    {

        private readonly ILogger<FaceCompareController> _logger;
        private readonly ICognitiveVisionService _cognitivevision;
        private readonly  IFirebaseService _firebase;

        public FaceCompareController(ILogger<FaceCompareController> logger,IFirebaseService firebase, ICognitiveVisionService cognitivevision)
        {
            _logger = logger;
            _cognitivevision = cognitivevision;
            _firebase = firebase;
        }

        [HttpPost]
        //[Authorize]
        public  async Task<IActionResult> FaceCompare(Guid FaceId1, Guid FaceId2)
        {
            try
            {
                VerifyResult verifyObjectResult = await _cognitivevision.VerifyTwoFaces( FaceId1,  FaceId2);
                Dictionary<string, object> dataObject = _firebase.TransformObjectCompare(verifyObjectResult, FaceId1,  FaceId2);
                await _firebase.SaveObject("face-compare",dataObject);
                return new ObjectResult(new {IsIdentical = verifyObjectResult.IsIdentical , Confidence = verifyObjectResult.Confidence }) { StatusCode = 200};
            }
            catch (System.Exception error)
            {
                return new ObjectResult(new { error = error.Message , facesCount = 0 }) { StatusCode = 500};
            }
            
        }

        
    }
}
