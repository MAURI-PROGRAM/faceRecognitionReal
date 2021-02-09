using Microsoft.Extensions.DependencyInjection;

namespace Api_face_recognition.Configurations
{
    /// <summary>
    /// CORS Configurations
    /// </summary>
    public static class CorsStartupConfiguration
    {
        /// <summary>
        /// Configure CORS services.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FaceRecognition", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.Build();
                });
            });
        }
    }
}