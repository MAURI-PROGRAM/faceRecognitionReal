using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Api_face_recognition.Domain;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.IO;

namespace Api_face_recognition.Services
{
    public interface ICognitiveVisionService
    {
        Task<List<DetectedFace>> DetectFaceRecognize( string url, string recognition_model);
        Task<List<DetectedFace>> DetectFaceRecognizeStream( Stream stream, string recognition_model);
        Task<VerifyResult> VerifyTwoFaces( Guid FaceId1, Guid FaceId2);
        Boolean EyesBlink( List<DetectedFace> facesDetected );
    }
}
