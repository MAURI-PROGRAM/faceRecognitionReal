using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Api_face_recognition.Domain;

namespace Api_face_recognition.Services
{
    public class JwtServices
    {
        public static JwtEntity GenerarTokenJWT(string secret, string ExpiresSeconds)
        {
            JwtEntity loginResponse = new JwtEntity();
            var id_token = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti, id_token)
                }),
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(ExpiresSeconds)),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            loginResponse.Token = tokenHandler.WriteToken(token);
            loginResponse.ExpireSeconds = ExpiresSeconds;
            loginResponse.RefreshToken = id_token;
            return loginResponse;
        }
    }
}
