using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api_face_recognition.Domain;
using Api_face_recognition.Services;
using Microsoft.Extensions.Options;

namespace Api_face_recognition.DependencyInjection
{
    public static class DependencyInjectorHost
    {
        public static void Configure(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var azureCognitive = serviceProvider.GetService<IOptions<AzureCognitiveConfiguration>>().Value;
            var firebase = serviceProvider.GetService<IOptions<FirebaseConfiguration>>().Value;
            services.AddSingleton<ICognitiveVisionService>(sp => new CognitiveVisionService(azureCognitive.EndPoint, azureCognitive.SubscriptionKey));
            services.AddSingleton<IFirebaseService>(sp => new FirebaseService(firebase.File,firebase.Credential,firebase.Collection));
        }
    }
}