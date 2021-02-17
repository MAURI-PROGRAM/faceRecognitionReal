namespace Api_face_recognition.Domain
{
    /// <summary>
    /// Constants for App Settings Configuration Sections
    /// </summary>
    public class AppSettingsSections
    {
        /// <summary>
        /// Token Configurations
        /// </summary>
        public static string Jwt = nameof(Jwt);

        /// <summary>
        /// Azure Cognitive Configurations
        /// </summary>
        public static string AzureCognitive = nameof(AzureCognitive);

        /// <summary>
        /// Azure Storage Configurations
        /// </summary>
        public static string AzureStorage = nameof(AzureStorage);
        /// <summary>
        /// Firebase Configurations
        /// </summary>
        public static string Firebase = nameof(Firebase);


        /// <summary>
        /// Custom Vision Configurations
        /// </summary>
        public static string CustomVision = nameof(CustomVision);
    }
}
