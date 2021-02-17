using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Api_face_recognition.Domain;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace Api_face_recognition.Services
{
    public interface IFirebaseService
    {
        Task<bool> SaveObject( string collectionName, Dictionary<string, object> dataObject);
        Dictionary<string, object> TransformObjectRecognition( string  IMAGE_URL, string FaceId);
        Dictionary<string, object> TransformObjectCompare( VerifyResult verifyObjectResult, Guid FaceId1, Guid FaceId2);
        Dictionary<string, object> TransformObjectImageUpload( string  IMAGE_URL);
        Dictionary<string, object> TransformObjectDetection(string Id, string predictions);
    }
}
