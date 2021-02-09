using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;

namespace Api_face_recognition.Configurations
{
    /// <summary>
    /// Swagger Startup Configuration
    /// </summary>
    public static class SwaggerStartupConfiguration
    {

        /// <summary>
        ///     Setup configuration for Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureService(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Api_face_recognition", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description ="JWT Authorization header usando el esquema Bearer. \r\n\r\n Ingresa 'Bearer' [espacio] y luego tu token en el text input de abajo.\r\n\r\nEjemplo: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        
        /// <summary>
        /// Configure the Swagger Endpoints and Middleware
        /// </summary>
        /// <param name="app"></param>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api_face_recognition v1"));

        }

        
    }
}
