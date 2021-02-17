using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Api_face_recognition.Domain;
using System.IO;
using Google.Cloud.Firestore;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;


namespace Api_face_recognition.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirestoreDb _db;
        public FirebaseService(string File ,string Credential, string Collection)
        {
            string path = Directory.GetCurrentDirectory()+File;
            Environment.SetEnvironmentVariable( Credential , path );
            _db = FirestoreDb.Create(Collection);
        }

        public  async Task<bool> SaveObject( string collectionName, Dictionary<string, object> dataObject)
        {
            try
            {
                CollectionReference faceCompare = _db.Collection(collectionName);
                await faceCompare.AddAsync(dataObject);
                return true;
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            
        } 

        public  Dictionary<string, object> TransformObjectCompare( VerifyResult verifyObjectResult, Guid FaceId1, Guid FaceId2)
        {
            Dictionary<string, object> dataObject = new Dictionary<string, object>();
            dataObject.Add("fecha",DateTime.UtcNow);
            dataObject.Add("Porcentaje", verifyObjectResult.Confidence);
            dataObject.Add("Verificacion", verifyObjectResult.IsIdentical);
            dataObject.Add("FaceId1", FaceId1.ToString());
            dataObject.Add("FaceId2", FaceId2.ToString());
            return dataObject;
            
        } 

        public  Dictionary<string, object> TransformObjectRecognition( string  IMAGE_URL, string FaceId)
        {
            Dictionary<string, object> dataObject = new Dictionary<string, object>();
            dataObject.Add("fecha",DateTime.UtcNow);
            dataObject.Add("url", IMAGE_URL);
            dataObject.Add("FaceId", FaceId );
            return dataObject;
            
        } 
        public  Dictionary<string, object> TransformObjectImageUpload( string  IMAGE_URL)
        {
            Dictionary<string, object> dataObject = new Dictionary<string, object>();
            dataObject.Add("fecha",DateTime.UtcNow);
            dataObject.Add("url", IMAGE_URL);
            return dataObject;
        }

        public  Dictionary<string, object> TransformObjectDetection(string Id, string predictions)
        {
            Dictionary<string, object> dataObject = new Dictionary<string, object>();
            dataObject.Add("fecha",DateTime.UtcNow);
            dataObject.Add("Id",Id);
            dataObject.Add("predictions", predictions);
            return dataObject;
        }


    }
}
