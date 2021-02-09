using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api_face_recognition.Domain;

namespace Api_face_recognition.Configurations
{
    /// <summary>
    /// Loads the Appsettings file configurations into the dependency injector.
    /// </summary>
    public static class LoadAppSettings
    {
        /// <summary>
        /// Load configuration into the injector.
        /// </summary>
        /// <param name="services">Injector service</param>
        /// <param name="configuration">Configurations object.</param>
        public static void IntoInjector(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<TokenStringsConfiguration>(
                configuration.GetSection(AppSettingsSections.Jwt));

            services.Configure<AzureCognitiveConfiguration>(
                configuration.GetSection(AppSettingsSections.AzureCognitive));

            services.Configure<AzureStorageConfiguration>(
                configuration.GetSection(AppSettingsSections.AzureStorage));
            
            services.Configure<FirebaseConfiguration>(
                configuration.GetSection(AppSettingsSections.Firebase));
        }
    }
}
