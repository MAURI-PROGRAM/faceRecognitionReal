using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Api_face_recognition.Domain;
using System;
using System.Text;


namespace Api_face_recognition.Configurations
{
    /// <summary>
    /// CORS Configurations
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// Configure Autenticacion services.
        /// </summary>
        /// <param name="services"></param>
        public static void AuthServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var tokenConfiguration = serviceProvider.GetService<IOptions<TokenStringsConfiguration>>().Value;
            var key = Encoding.ASCII.GetBytes(tokenConfiguration.Key);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                });

        }
    }
}
