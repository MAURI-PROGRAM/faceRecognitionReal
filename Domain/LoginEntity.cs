using System;

namespace Api_face_recognition.Domain
{
    public class JwtEntity 
    {
        public string Token { get; set; }
        public string ExpireSeconds { get; set; }
        public string RefreshToken { get; set; }
    }
}
