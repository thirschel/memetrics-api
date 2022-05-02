namespace MeMetrics.Application.Models
{
    /*
    This file should contain all the keys in the environment variables
    */
    public class EnvironmentConfiguration
    {
        public string ENV { get; set; }
        public string LOG_LEVEL { get; set; }
        public string DB_CONNECTION_STRING { get; set; }
        public string PRIMARY_API_KEY { get; set; }
        public string SECONDARY_API_KEY { get; set; }
        public string ALLOWED_ORIGIN { get; set; }
        public string BLOB_STORAGE_CONNECTION_STRING { get; set; }
        public string GOOGLE_PHOTO_REFRESH_TOKEN { get; set; }
        public string GOOGLE_CLIENT_ID { get; set; }
        public string GOOGLE_CLIENT_SECRET { get; set; }
        public string GOOGLE_ALBUM_ID { get; set; }
    }
}
