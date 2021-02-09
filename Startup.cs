using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Api_face_recognition.Configurations;
using Api_face_recognition.DependencyInjection;

namespace Api_face_recognition
{
    /// <summary>
    /// Application Startup Configuration
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            /* Configuration = configuration; */
        }

        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //Ini Habilitar Cors
            services.AddCors();

            services.AddControllers();
            LoadAppSettings.IntoInjector(services, Configuration);
            DependencyInjectorHost.Configure(services);
            SwaggerStartupConfiguration.ConfigureService(services);
            AuthenticationConfiguration.AuthServices(services);
            CorsStartupConfiguration.ConfigureServices(services);
            
            //services.AddSingleton<ISingleton, Singleton>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                SwaggerStartupConfiguration.Configure(app);
               
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();

            //Habilitar Cors
            //app.UseCors();
            app.UseCors("FaceRecognition");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
