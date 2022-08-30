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
    }
}
