using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;
using Api_face_recognition.Domain;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Api_face_recognition.Services
{
    public class CustomVisionService : ICustomVisionService
    {
        private readonly HttpClient _client;
        private readonly string _url;
        public CustomVisionService(string PredictionKey ,string Url)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Prediction-Key", PredictionKey);
            _url = Url;
        }

        public async Task<HttpResponseMessage> IdentifyCedula(string filePath)
        {    
            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(filePath);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await _client.PostAsync(_url, content);
            }
            return response;
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

       

    }
}
