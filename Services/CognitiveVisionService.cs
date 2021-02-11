using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Api_face_recognition.Domain;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace Api_face_recognition.Services
{
    public class CognitiveVisionService:ICognitiveVisionService
    {
        private readonly IFaceClient _client;
        public CognitiveVisionService(string endpoint, string key)
        {
            _client = new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        public  async Task<List<DetectedFace>> DetectFaceRecognize( string url, string recognition_model)
        {
            IList<FaceAttributeType?> faceAttributes =
                new FaceAttributeType?[]
                {
                    FaceAttributeType.Age, FaceAttributeType.Gender,

                };
            IList<DetectedFace> detectedFaces = await _client.Face.DetectWithUrlAsync(url, recognitionModel: recognition_model,returnFaceAttributes: faceAttributes, detectionModel: DetectionModel.Detection01);
            return detectedFaces.ToList();
        } 

        public  async Task<List<DetectedFace>> DetectFaceRecognizeStream( Stream stream, string recognition_model)
        {
            IList<DetectedFace> detectedFaces = await _client.Face.DetectWithStreamAsync(stream, true, true, null);
            return detectedFaces.ToList();
        } 

        public  async Task<VerifyResult> VerifyTwoFaces( Guid FaceId1, Guid FaceId2)
        {
            VerifyResult verifyObjectResult = await _client.Face.VerifyFaceToFaceAsync(FaceId1, FaceId2);
            return verifyObjectResult;
        }

        public  Boolean EyesBlink( List<DetectedFace> facesDetected , Double EYE_AR_THRESH)
        {
            var facelandmarks = facesDetected[0].FaceLandmarks;
                    Double  difLeftX = Math.Sqrt(
                                    Math.Pow(facelandmarks.EyeLeftInner.X-facelandmarks.EyeLeftOuter.X,2)+
                                    Math.Pow(facelandmarks.EyeLeftInner.Y-facelandmarks.EyeLeftOuter.Y,2));
                    Double  difLeftY = Math.Sqrt(
                                    Math.Pow(facelandmarks.EyeLeftBottom.X-facelandmarks.EyeLeftTop.X,2)+
                                    Math.Pow(facelandmarks.EyeLeftBottom.Y-facelandmarks.EyeLeftTop.Y,2));

                    Double difRightX = Math.Sqrt(
                                    Math.Pow(facelandmarks.EyeRightInner.X-facelandmarks.EyeRightOuter.X,2)+
                                    Math.Pow(facelandmarks.EyeRightInner.Y-facelandmarks.EyeRightOuter.Y,2));
                    Double difRightY = Math.Sqrt(
                                    Math.Pow(facelandmarks.EyeRightBottom.X-facelandmarks.EyeRightTop.X,2)+
                                    Math.Pow(facelandmarks.EyeRightBottom.Y-facelandmarks.EyeRightTop.Y,2));
                    
                    Double leftEAR = difLeftY / difLeftX;
                    Double rightEAR = difRightY/difRightX;
                    bool leftBlink = leftEAR < EYE_AR_THRESH;
                    bool rightBlink = rightEAR < EYE_AR_THRESH;
            return leftBlink || rightBlink;
        }
        
    }
}
