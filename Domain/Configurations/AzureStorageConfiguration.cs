namespace Api_face_recognition.Domain
{
    public class AzureStorageConfiguration
    {
        /// <summary>
        /// Azure storage Configuration String
        /// </summary>
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string UrlStorage { get; set; }
        
    }
}
